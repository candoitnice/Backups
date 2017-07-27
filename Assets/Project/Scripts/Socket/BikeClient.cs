using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class BikeClient
{

    public TcpClient tcpClient;
    public string localIp { get; set; }

    private byte[] data;
    private Thread RecivedThread;
    public Thread SendDataThread;
    bool isRecived;

    public BikeClient(TcpClient tcpClient)
    {
        isRecived = true;
        this.tcpClient = tcpClient;
        localIp = tcpClient.Client.RemoteEndPoint.ToString();
        data = new byte[tcpClient.ReceiveBufferSize];//限制接受数据大小已最大数据量定义
        Debug.Log(localIp);
        RecivedThread = new Thread(new ThreadStart(OnRecivedData));
        RecivedThread.Start();
        SendDataThread = new Thread(new ThreadStart(SendData));
        SendDataThread.Start();
    }
    private void OnRecivedData()
    {
        while (isRecived)
        {
            try
            {
                int count = 0;
                try
                {
                    Socket socket = tcpClient.Client;
                    if (socket == null) continue;
                    data = new byte[65536];
                    count = socket.Receive(data);
                    if (count < 1)//接收到数据小于1时（数据异常）
                    {
                        close();//关闭线程；
                    }
                    else
                    {
                        byte[] buff = new byte[count];
                        Array.Copy(data, buff, count);
                        string readData = Encoding.UTF8.GetString(buff);
                        string r = readData.Split(';')[0];
                        string[] info = r.Split(':');
                        //Debug.Log(readData);

                        switch (info[0])
                        {
                            case "REG"://注册
                                EventManager.Instance._OnREG(info);
                                break;
                            case "DAT"://训练
                                EventManager.Instance._OnDAT(info);
                                break;
                            case "RPT"://报告
                                EventManager.Instance._OnRPT(info);
                                break;
                            case "COD"://状态
                                EventManager.Instance._OnCOD(info);
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Data);
                    close();//关闭线程；
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Data);
                close();//关闭线程；
            }
        }
    }

    public void SendData()
    {
        while (isRecived)
        {
            Thread.Sleep(200);
            try
            {
                if (MySocket.Instance.listSendValue.Count > 0)
                {
                    SendData(MySocket.Instance.listSendValue[0]);
                    MySocket.Instance.listSendValue.Remove(MySocket.Instance.listSendValue[0]);
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
       }
    }

    //发送数据
    public void SendData(string info)
    {
        try
        {
            byte[] buff = Encoding.UTF8.GetBytes(info);
            if (tcpClient != null)
            {
                if (tcpClient.Connected)
                {
                    tcpClient.Client.Send(buff);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            close();
        }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public  void close()
    {
        Recovery.GameManager.instance.queue.Enqueue("BikeClose");
        isRecived = false;
        if (tcpClient != null)
        {
            tcpClient.Close();
            tcpClient = null;
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
        Debug.Log("关闭客户端");
    }
}