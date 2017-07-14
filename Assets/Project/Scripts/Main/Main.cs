using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;


    public Recovery.Type.GameEditionType GEType =Recovery.Type.GameEditionType.Adult;

    void Awake()
    {
        Instance = this;
           Resolution[] resolutions = Screen.resolutions;
        foreach (var item in resolutions)
        {
            if (item.height != 1920 && item.width != 1080)
            {
                Screen.SetResolution(540, 960, false);
            }
            else
            {
                Screen.SetResolution(1080, 1920, true);
                break;
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        UIUpdateManaget.Instance.InitUI();
        Http_WWW.Instance.Init();
        InfoHandle.Instance.InitEvent();
        EventManager.Instance.onGameUpdate+=GameManager.instance.QueuePlay;
        EventManager.Instance.onGameInit += Init;
    }
    //Process pr;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <returns></returns>
    public void Init()
    {
        switch (Recovery.GameData.Instance.gameModeType)
        {
            case Recovery.Type.GameModeType.single:
                GameManager.instance.InitGameSingle();
                break;
            case Recovery.Type.GameModeType.many:
                    GameManager.instance.InitGameMany();
                break;
        }
        EventManager.Instance.onGameInit -= Init;
    }



    // Update is called once per frame
    void Update()
    {
        EventManager.Instance._OnGameUpdate();
    }

    /// <summary>
    /// 脚本失效时运行
    /// </summary>
    void OnApplicationQuit()
    {
        //pr.Kill();
        MySocket.Instance.close();
        MyClient.Instance.close();
        InfoHandle.Instance.CloseEvent();
        EventManager.Instance.onGameUpdate -= GameManager.instance.QueuePlay;
        EventManager.Instance._OnApplicationQuit();
    }
}
