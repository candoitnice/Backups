using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.IO;
using SportCommon;
using System.Collections.Generic;

namespace SportClient
{
    public class ClientPeerBase
    {
        public IPEndPoint ip { get; set; }
        public IClient client { get; set; }

        public TcpClient tcpClient { get; set; }

        private Thread OnRevicveMsgThread { get; set; }
        private bool isRunning = false;

        public ConnectState state = ConnectState.Close;
        public Thread OnSendMsgThread { get; private set; }
        public Thread OnSendGDataThread { get; private set; }

        ThreadsPool pool;
        public ClientPeerBase(IPEndPoint ip, IClient client)
        {
            pool = ThreadsPool.Instance;
            isRunning = true;
            this.ip = ip;
            this.client = client;

            OnSendMsgThread = new Thread(new ThreadStart(OnSendPData));
            OnSendMsgThread.Start();
            OnSendGDataThread = new Thread(new ThreadStart(OnSendGData));
            OnSendGDataThread.Start();

            OnThreadStart(OnRevicveMsgThread, OnReciveData);
        }

        private void OnSendPData()
        {

            while (isRunning)
            {
                lock (MsgQueue.mQueue)
                {
                    if (MsgQueue.mQueue.Count <= 0 || tcpClient == null) continue;

                    try
                    {
                        if (!tcpClient.Connected) continue;
                        Parameter p = MsgQueue.mQueue.Dequeue();
                        byte[] bytes = SerializeTool.SerializeParam(p);
                        byte[] length = new byte[4];
                        int len = bytes.Length;
                        length[0] = (byte)(len >> 24);
                        length[1] = (byte)(len >> 16);
                        length[2] = (byte)(len >> 8);
                        length[3] = (byte)(len);
                        tcpClient.Client.Send(length);
                        tcpClient.GetStream().Flush();
                        tcpClient.Client.Send(bytes);
                        tcpClient.GetStream().Flush();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        OnDisconnect();
                    }
                }
            }
        }
        public void OnSendGData()
        {
            while (isRunning)
            {
                Thread.Sleep(50);
                if (Recovery.GameData.Instance.GDP_Bike != null)
                {
                    Parameter p = new Parameter();
                    p.OperaCode = (byte)Operation.GameSyn;
                    p.Parameters = new Dictionary<byte, object>();
                    ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.GDATA);
                    ParameterTool.AddParmerer(p.Parameters, SubCode.GDATA, Recovery.GameData.Instance.GDP_Bike);
                    lock (MsgQueue.mQueue)
                        MsgQueue.mQueue.Enqueue(p);
                }
            }
        }

        private void OnReciveData()
        {

            MemoryStream stream = new MemoryStream(65536);
            while (isRunning)
            {
                stream.Position = 0L;
                stream.SetLength(0L);
                try
                {
                    if (tcpClient != null && tcpClient.Connected)
                    {
                        int offset = 0;
                        int num2 = 0;
                        byte[] buffer = new byte[4];
                        Socket socket = tcpClient.Client;
                        while (offset < 4)
                        {
                            num2 = socket.Receive(buffer, offset, 4 - offset, SocketFlags.None);
                            offset += num2;
                            if (num2 == 0)
                            {
                                throw new SocketException(0x2746);
                            }
                        }
                        int size = (((buffer[0] << 0x18) | (buffer[1] << 0x10)) | (buffer[2] << 8)) | buffer[3];

                        offset = 0;
                        buffer = new byte[size];
                        while (offset < size)
                        {
                            num2 = socket.Receive(buffer, offset, size - offset, SocketFlags.None);
                            offset += num2;
                            if (num2 == 0)
                            {
                                throw new SocketException(0x2746);
                            }
                        }
                        stream.Write(buffer, 0, offset);
                        if (stream.Length > 0L)
                        {
                            if (client != null)
                            {
                                try
                                {
                                    Parameter p = SerializeTool.DeserializeParam(stream.ToArray());
                                    if (p != null)
                                    {
                                        ArgmentParam ar = new ArgmentParam();
                                        ar.param = p;
                                        ar.client = this.client;

                                        ThreadsPool.Instance.queue.Enqueue(ar);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        else
                        {

                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    OnDisconnect();
                }
            }

        }

        public void SendParamer(Parameter param)
        {
            lock (MsgQueue.mQueue)
                MsgQueue.mQueue.Enqueue(param);
            //if (tcpClient != null)
            //{
            //    try
            //    {
            //        if (tcpClient.Client == null) return;
            //        byte[] buff = SerializeTool.SerializeParam(param);
            //        byte[] length = new byte[4];
            //        int len = buff.Length;
            //        length[0] = (byte)(len >> 24);
            //        length[1] = (byte)(len >> 16);
            //        length[2] = (byte)(len >> 8);
            //        length[3] = (byte)(len);
            //        tcpClient.Client.Send(length);
            //        tcpClient.Client.Send(buff);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.Log(ex.Message);
            //        CloseTcp();
            //    }
            //}
        }


        public void OnDisconnect()
        {
            if (tcpClient != null) tcpClient.Close();
            tcpClient = null;
        }

        private void CloseTcp()
        {
            tcpClient.Close();

            state = ConnectState.Closing;
            client.OnConnectStateChange(state);
        }

        public void OnConnect()
        {
            if (state == ConnectState.Connecting) return;
            state = ConnectState.Open;
            if (tcpClient == null)
                tcpClient = new TcpClient();
            else
            {
                tcpClient.Close();
            }
            try
            {
                state = ConnectState.Connecting;

                tcpClient.Connect(this.ip);
                state = ConnectState.Connected;
            }
            catch (Exception ex)
            {
                state = ConnectState.Error;
            }
            client.OnConnectStateChange(state);

        }

        public void OnClientClose()
        {
            CloseTcp();
            isRunning = false;
            state = ConnectState.Close;
            OnThreadAbort(OnRevicveMsgThread);
            OnThreadAbort(OnSendMsgThread);
            OnThreadAbort(OnSendGDataThread);
            client.OnConnectStateChange(state);
            GameManager.instance.queue.Enqueue("CloentClose");
        }

        void OnThreadAbort(Thread thread)
        {
            if (thread != null)
            {
                Debug.Log(thread.Name + "  Abort");
                thread.Abort();
            }
            thread = null;
        }





        void OnThreadStart(Thread thread, Action action)
        {
            if (thread != null) thread.Abort();
            thread = null;
            thread = new Thread(new ThreadStart(action));
            thread.Start();
            thread.IsBackground = true;
        }
    }
}
