using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.NetworkInformation;

public class MySocket
{
    private static MySocket _Instance;
    public static MySocket Instance
    {
        get
        {
            if (_Instance == null) _Instance = new MySocket(); return _Instance;
        }
    }


    public BikeClient bikeClient;
    public List<string> listSendValue = new List<string>();
    NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();



    #region 接收自行车数据用socket服务器
    /// <summary>
    /// 接收自行车数据用socket服务器
    /// </summary>
    TcpListener serverSocket;
    private Thread ServerSocketThread;

    /// <summary>
    /// 获取本地所有的IP
    /// </summary>
    /// <returns>V4，V6</returns>
    public static IEnumerable<IPAddress> GetIPs()
    {
        try
        {
            IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
            return addressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork);
        }
        catch (Exception EX)
        {
            Debug.Log(EX.Message);
            return new IPAddress[0];
        }
    }

    /// <summary>
    /// 开启服务器
    /// </summary>
    public void OnServer(string ip, string port)
    {
        try
        {
            //ip = GetIPv4.GetLocalIP();
            //if (ip.Substring(0, 3) == "192" || ip.Substring(0, 3) == "127")
            //{
            //}
            //else
            //    ip = "127.0.0.1";

            Debug.Log(ip);
            IPAddress ipa = IPAddress.Parse(ip);
            IPEndPoint ipe = new IPEndPoint(ipa,int.Parse(port));
            serverSocket = new TcpListener(ipa,int.Parse(port));
            serverSocket.Start(10);
            Debug.Log("开启服务器" + serverSocket.LocalEndpoint.ToString());
            ServerSocketThread = new Thread(new ThreadStart(Accept));
            ServerSocketThread.Start();


        }
        catch (Exception)
        {
            Debug.Log("启动监听" + serverSocket.LocalEndpoint.ToString());
        }
    }

    private void Accept()
    {
        while (true)
        {
            try
            {
                BikeClient client = new BikeClient(serverSocket.AcceptTcpClient());
                bikeClient = client;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }


    public void close()
    {
        if (serverSocket != null)
        {
            serverSocket.Server.Close();
            serverSocket = null;
            Debug.Log("退出");
        }
        if (ServerSocketThread != null)
        {
            ServerSocketThread.Abort();
            ServerSocketThread = null;
            Debug.Log("关闭线程");
        }
        if (bikeClient!=null)
        bikeClient.close();
    }
    #endregion




}
