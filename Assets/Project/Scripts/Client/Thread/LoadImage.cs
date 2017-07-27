using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LoadImage
{
    public static void loadImage(string path, RawImage image,Action<object,object>callback)
    {
        new Thread(() =>
        {
            byte[] data = File.ReadAllBytes(path);
            Argment ar = new Argment();
            ar.data = data;
            ar.send = image;
            ar.callback = callback;
            //ThreadsPool.Instance.dataQueue.Enqueue(ar);
        }).Start();
    }
}
