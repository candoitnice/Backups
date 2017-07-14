using SportClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArgmentParam
{
    public IClient client;
    public Parameter param;
}
public class Argment
{
    public object data;
    public object send;
    public Action<object,object> callback;
}
public class ThreadsPool : MonoBehaviour
{
    private static ThreadsPool instance;
    public static ThreadsPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("ThreadPool").AddComponent<ThreadsPool>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }
    public Queue<ArgmentParam> queue = new Queue<ArgmentParam>();

    public Queue<Argment> dataQueue = new Queue<Argment>();

    private void Update()
    {
        if (queue.Count > 0)
        {
            ArgmentParam ar = queue.Dequeue();
            try
            {
                if (ar.client != null)
                    ar.client.OnRecivedData(ar.param);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        if (dataQueue.Count > 0)
        {
            Argment ar = dataQueue.Dequeue();
            try
            {
                if(ar.callback!=null)
                {
                    ar.callback(ar.data,ar.send);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

    private void OnDestroy()
    {
        queue.Clear();
    }
}
