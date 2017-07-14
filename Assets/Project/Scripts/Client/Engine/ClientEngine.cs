using SportClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Net;

public class ClientEngine : IClient
{
    public ClientEngine()
    {
        IsMaster = false;
        //if (!init)
        //{
        //    init = true;
        //    TimeTick.Instance.AddDelayEvent(Init, null, 0.1f);
        //}
        EventManager.Instance.onApplicationQuit+= Instance_OnApplicationQuitEvent;
    }

    private void Instance_OnApplicationQuitEvent()
    {
        Close();
        EventManager.Instance.onApplicationQuit -= Instance_OnApplicationQuitEvent;
    }

    public Dictionary<Operation, HandleBase> handles = new Dictionary<Operation, HandleBase>();
    private bool init = false;
    private static ClientEngine instance;

    public ClientPeerBase client;

    public GameServerState serverState = GameServerState.Disconnected;

    public bool IsMaster { get; set; }

    public static ClientEngine Instance
    {
        get
        {
            if (instance == null) instance = new ClientEngine();
            return instance;
        }
    }

    private void Init(object obj)
    {
        Resgister();
    }

    public void Connect(IPEndPoint endpoint)
    {
        if (!init)
        {
            init = true;
            Resgister();
            client = new ClientPeerBase(endpoint, this);
        }
        client.OnConnect();
    }

    private void Resgister()
    {
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var item in types)
        {
            if (typeof(HandleBase).IsAssignableFrom(item) && item.Name != typeof(HandleBase).Name)
            {
                Activator.CreateInstance(item);
            }
        }
    }

    public void OnConnectStateChange(ConnectState state)
    {
        if (state == ConnectState.Connected)
        {
          
        }
    }

    public void OnDisconnect()
    {
        Debug.Log("OnDisconnect");
    }

    public void OnRecivedData(Parameter pararme)
    {
        HandleBase handle;

        handles.TryGetValue((Operation)pararme.OperaCode, out handle);

        if (handle != null)
        {
            try
            {
                handle.Handle(pararme);
            }
            catch (Exception ex)
            {
                Debug.Log(handle.OpCode + ex.Message);
            }
        }
    }

    public void Close()
    {
        if (client != null)
            client.OnClientClose();
    }

    public void OnSendParamer(Parameter p)
    {

        if (client != null)
            client.SendParamer(p);
    }
}
