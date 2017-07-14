using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MyClient
{

    private static MyClient _Instance;
    public static MyClient Instance
    {
        get
        {
            if (_Instance == null) _Instance = new MyClient(); return _Instance;
        }
    }


    #region 同步信息客户端

    /// <summary>
    /// 客户端
    /// </summary>
    public TcpClient client { get; set; }
    public string localIp { get; set; }
    private byte[] data;
    private Thread RecivedThread;
    private Thread SendDataThread;
    bool isRunning = false;
    public string number = "";

    public Queue<string> queue=new Queue<string>();


    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void Connect(string ip, string port)
    {
        client = new TcpClient();
        try
        {
            isRunning = true;
            IPAddress ipa = IPAddress.Parse(ip);
            client.Connect(ipa, int.Parse(port));
            localIp = client.Client.RemoteEndPoint.ToString();
            Debug.Log("连接服务器成功");

            data = new byte[client.ReceiveBufferSize];//限制接受数据大小已最大数据量定义
            RecivedThread = new Thread(new ThreadStart(OnRecivedData));
            RecivedThread.Start();

            SendDataThread = new Thread(new ThreadStart(SendData));
            SendDataThread.Start();

            InfoData info = new InfoData();
            //if (Main.Instance.GEType == Type.GameEditionType.children)
            //    info.Gtype = GameType.C_Bike;
            //else
            //    info.Gtype = GameType.Bike;
            queue.Enqueue(JsonConvert.SerializeObject(info));
        }
        catch (Exception ex)
        {
            GameManager.instance.queue.Enqueue("Close");
            Debug.Log("连接发生错误" + ex.Message);
            close();//关闭线程；
        }
    }

    //异步读取数据触发方法
    private void OnRecivedData()
    {
        MemoryStream stream = new MemoryStream(65536);

        while (isRunning)
        {
            stream.Position = 0L;
            stream.SetLength(0L);
            try
            {
                if (client != null && client.Connected)
                {
                    int offset = 0;
                    int num2 = 0;
                    byte[] buffer = new byte[4];
                    Socket _socket = client.Client;
                    {
                        while (offset < 4)
                        {
                            num2 = _socket.Receive(buffer, offset, 4 - offset, SocketFlags.None);
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
                            num2 = _socket.Receive(buffer, offset, size - offset, SocketFlags.None);
                            offset += num2;
                            if (num2 == 0)
                            {
                                throw new SocketException(0x2746);
                            }
                        }
                        stream.Write(buffer, 0, offset);
                        if (stream.Length > 0L)
                        {
                            string readData = Encoding.UTF8.GetString(stream.ToArray());
                        }
                        else
                        {
                            //Debug.Log("错误");
                          //  close();//关闭线程；
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string readData = Encoding.UTF8.GetString(stream.ToArray());
                Debug.Log("错误" + readData + "" + stream.Length);
                close();//关闭线程；
            }
        }
    }

    //发送数据
    public void SendData()
    {
        while (isRunning)
        {
            
                if (queue.Count <= 0 || client == null) continue;

                try
                {
                    if (!client.Connected) continue;
                    string info = queue.Dequeue();

                    //Debug.Log(info);
                    byte[] buff = Encoding.UTF8.GetBytes(info);
                    byte[] length = new byte[4];
                    int len = buff.Length;
                    length[0] = (byte)(len >> 24);
                    length[1] = (byte)(len >> 16);
                    length[2] = (byte)(len >> 8);
                    length[3] = (byte)(len);

                    client.Client.Send(length);
                    client.GetStream().Flush();
                    client.Client.Send(buff);
                    client.GetStream().Flush();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    close();
                }
            
        }
    }

    /// <summary>
    ///关闭
    /// </summary>
    public void close()
    {
        //GameManager.instance.queue.Enqueue("Close");
        isRunning = false;
        if (client != null)
        {
            client.Close();
            client = null;
        }
        if (RecivedThread != null)
        {
            RecivedThread.Abort();
            RecivedThread = null;
        }
        if (SendDataThread != null)
        {
            SendDataThread.Abort();
            SendDataThread = null;
        }

    }


    #endregion

}
