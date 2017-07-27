using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
                instance = new EventManager();
            return instance;
        }
    }



    /// <summary>
    /// 游戏相关事件委托
    /// </summary>
    public delegate void GameEvnem();
    /// <summary>
    /// 游戏结束事件
    /// </summary>
    public event GameEvnem onGameOver;
    /// <summary>
    /// 游戏开始事件
    /// </summary>
    public event GameEvnem onPlayGame;
    /// <summary>
    /// 游暂停始事件
    /// </summary>
    public event GameEvnem onSpotGame;
    /// <summary>
    /// 游始继续事件
    /// </summary>
    public event GameEvnem onContinueGame;
    /// <summary>
    /// 游始重新开始事件
    /// </summary>
    public event GameEvnem onGameAgain;
    /// <summary>
    ///实时更新Update
    /// </summary>
    public event GameEvnem onGameUpdate;
    /// <summary>
    /// 初始化
    /// </summary>
    public event GameEvnem onGameInit;
    /// <summary>
    /// 脚本实效事件
    /// </summary>
    public event GameEvnem onApplicationQuit;


    /// <summary>
    /// 脚本实效事件
    /// </summary>
    public void _OnApplicationQuit() { if (onApplicationQuit != null) onApplicationQuit(); }
    /// <summary>
    /// 初始化
    /// </summary>
    public void _OnGameInit() { if (onGameInit != null) onGameInit(); }
    /// <summary>
    ///实时更新Update
    /// </summary>
    public void _OnGameUpdate()
    {
        if (onGameUpdate != null) onGameUpdate();
    }
    /// <summary>
    /// 游始重新开始事件
    /// </summary>
    public void _OnGameAgain()
    {
        if (onGameAgain != null) onGameAgain();
    }
    /// <summary>
    /// 游戏结束事件
    /// </summary>
    public void _OnGameOver()
    {
        if (onGameOver != null) onGameOver();
    }
    /// <summary>
    /// 游戏开始事件
    /// </summary>
    public void _OnPlayGame()
    {
        if (onPlayGame != null) onPlayGame();
    }
    /// <summary>
    /// 游暂停始事件
    /// </summary>
    public void _OnSpotGame()
    {
        if (onSpotGame != null) onSpotGame();
    }
    /// <summary>
    /// 游始继续事件
    /// </summary>
    public void _OnContinueGame()
    {
        if (onContinueGame != null) onContinueGame();
    }





    /// <summary>
    /// 自行车相关委托
    /// </summary>
    public delegate void Bicycle(string[] value);
    /// <summary>
    ///单片机注册成功事件
    /// </summary>
    public event Bicycle OnREG;
    /// <summary>
    ///单片机训练数据事件
    /// </summary>
    public event Bicycle OnDAT;
    /// <summary>
    ///单片机传送训练报告数据事件
    /// </summary>
    public event Bicycle OnRPT;
    /// <summary>
    ///单片机状态数据事件
    /// </summary>
    public event Bicycle OnCOD;



    /// <summary>
    ///单片机注册成功事件
    /// </summary>
    public void _OnREG(string[] value) { if (OnREG != null) OnREG(value); }
    /// <summary>
    ///单片机训练数据事件
    /// </summary>
    public void _OnDAT(string[] value) { if (OnDAT != null) OnDAT(value); }
    /// <summary>
    ///单片机传送训练报告数据事件
    /// </summary>
    public void _OnRPT(string[] value) { if (OnRPT != null) OnRPT(value); }
    /// <summary>
    ///单片机状态数据事件
    /// </summary>
    public void _OnCOD(string[] value) { if (OnCOD != null) OnCOD(value); }




    /// <summary>
    /// 接收信息相关委托
    /// </summary>
    public delegate void BikeSocketData(GameDataPacketBike value);

    /// <summary>
    ///接收GameData类型信息相关成功事件
    /// </summary>
    public event BikeSocketData OnGameData;
    /// <summary>
    /// 接收信息相关委托
    /// </summary>
    public delegate void HttpSocketData(HttpInfo value);
    /// <summary>
    ///接收GameData类型信息相关成功事件
    /// </summary>
    public event HttpSocketData OnGameSET;

    /// <summary>
    /// 接收信息相关委托
    /// </summary>
    public delegate void ZCInfoSocketData(List<BikeZCInfo> value);
    /// <summary>
    /// 注册服务器成功
    /// </summary>
    public event ZCInfoSocketData onGameZCData;
    /// <summary>
    /// 接收信息相关委托
    /// </summary>
    public delegate void OnDisconnectData(BikeZCInfo value);
    /// <summary>
    ///断开连接
    /// </summary>
    public event OnDisconnectData OnDisconnect;



    /// <summary>
    ///接收GameData类型信息相关成功事件
    /// </summary>
    public void _OnGameData(GameDataPacketBike value) { if (OnGameData != null) OnGameData(value); }
    /// <summary>
    ///接收GameData类型信息相关成功事件
    /// </summary>
    public void _OnGameSET(HttpInfo value) { if (OnGameSET != null) OnGameSET(value); }
    /// <summary>
    ///接收GameData类型信息相关成功事件
    /// </summary>
    public void _OnGameZCData(List<BikeZCInfo> value) { if (onGameZCData != null) onGameZCData(value); }
    /// <summary>
    ///断开连接
    /// </summary>
    public void _OnDisconnect(BikeZCInfo value) { if (OnDisconnect != null) OnDisconnect(value); }
}
