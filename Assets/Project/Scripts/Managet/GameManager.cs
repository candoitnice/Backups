using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    public Material roadMate;
    public Texture[] roadText;



    /// <summary>
    ///初始化单机游戏
    /// </summary>
    public void InitGameSingle()
    {
        roadMate.mainTexture = roadText[0];
        MySocket.Instance.OnServer(Recovery.GameData.Instance.bikeIp, Recovery.GameData.Instance.bikePort);
        UIUpdateManaget.Instance.Single.SetActive(true);
        UIUpdateManaget.Instance.Many.SetActive(false);
        DifficultyConversion();
    }
    /// <summary>
    ///初始化多人机游戏
    /// </summary>
    public void InitGameMany()
    {
        roadMate.mainTexture = roadText[1];
        MySocket.Instance.OnServer(Recovery.GameData.Instance.bikeIp, Recovery.GameData.Instance.bikePort);

        IPAddress ip = IPAddress.Parse(Recovery.GameData.Instance.serverIp);
        IPEndPoint end = new IPEndPoint(ip,int.Parse(Recovery.GameData.Instance.serverPort));
        ClientEngine.Instance.Connect(end);

        Parameter p = new Parameter();
        p.OperaCode = (byte)Operation.Register;
        p.Parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.GameType);
        ParameterTool.AddParmerer(p.Parameters, SubCode.GameType, GameType.Bike);
        ClientEngine.Instance.OnSendParamer(p);

        EventManager.Instance.onPlayGame += OnPlayGame;
        UIUpdateManaget.Instance.Single.SetActive(false);
        UIUpdateManaget.Instance.Many.SetActive(true);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void GameQuit()
    {
        Application.Quit();
    }
    // 游戏结束
    public void GameOver()
    {
        GameStop();
        for (int i = 0; i < 5; i++)
        {
            MySocket.Instance.listSendValue.Add("STP:" + Recovery.GameData.Instance.Number + ";");
        }
        MySocket.Instance.listSendValue.Add("2");
        for (int i = 0; i < 5; i++)
        {
            MySocket.Instance.listSendValue.Add("END:" + Recovery.GameData.Instance.Number + ";");
        }
        MySocket.Instance.listSendValue.Add("2");

        if (Recovery.GameData.Instance.gameModeType ==Recovery.Type.GameModeType.many)
        {
            UIUpdateManaget.Instance.SetRankingList();
        }
        else
        {
            UIUpdateManaget.Instance.gameOver.gameObject.SetActive(true);
            UIUpdateManaget.Instance.gameOver.OnPlay();
        }
        Recovery.GameData.Instance.isPlayGame = false;
    }
    //游戏暂停
    public void GameStop()
    {
        for (int i = 0; i < 5; i++)
        {
            MySocket.Instance.listSendValue.Add("STP:" + Recovery.GameData.Instance.Number + ";");
        }
        MySocket.Instance.listSendValue.Add("2");
        Recovery.GameData.Instance.isStopGame = true;
    }
    //游戏继续
    public void GameContinue()
    {
        for (int i = 0; i < 5; i++)
        {
            MySocket.Instance.listSendValue.Add("REQ:" + Recovery.GameData.Instance.Number + ";");
        }
        Recovery.GameData.Instance.isStopGame = false;
    }
    /// <summary>
    ///重新游戏
    /// </summary>
    public void GameAgain()
    {
        Debug.Log("重新游戏");
        Recovery.GameData.Instance.isPlayGame = false;
        Recovery.GameData.Instance.isStopGame = false;
        Recovery.GameData.Instance.currentGameTime = 0;
        Recovery.GameData.Instance.mainPlayer.GameAgain();
        UIUpdateManaget.Instance.Back.SetActive(true);
        lock (queue)
        {
            queue.Enqueue("Play");
        }
    }

    /// <summary>
    ///开始游戏
    /// </summary>
    public void OnPlayGame()
    {
        lock (queue)
        {
            queue.Enqueue("Play");
        }
        EventManager.Instance.onPlayGame -= OnPlayGame;
    }
    public void Button_PlayGame()
    {
        if (Recovery.GameData.Instance.gameModeType == Recovery.Type.GameModeType.many)
        {
            DifficultyConversion();

            Parameter p = new Parameter();
            p.OperaCode = (byte)Operation.GameSyn;
            p.Parameters = new Dictionary<byte, object>();
            ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.SET);
            ParameterTool.AddParmerer(p.Parameters, SubCode.SET, Http_WWW.Instance.varConfig);
            ClientEngine.Instance.OnSendParamer(p);

            Parameter _p = new Parameter();
            _p.OperaCode = (byte)Operation.GameSyn;
            _p.Parameters = new Dictionary<byte, object>();
            ParameterTool.AddParmerer(_p.Parameters, PameraCode.SubCode, SubCode.GPLAY);
            ParameterTool.AddParmerer(_p.Parameters, SubCode.GPLAY,"Play");
            ClientEngine.Instance.OnSendParamer(_p);
        }
    }
    public void TimeDome_Playgame()
    {
        Recovery.GameData.Instance.isPlayGame = true;
        Camera.main.GetComponent<CameraFlow>().target = Recovery.GameData.Instance.mainPlayer.target;
        Camera.main.GetComponent<CameraFlow>().enabled = true;

        EventManager.Instance.onGameUpdate += CountDown;
        DateTime data = DateTime.Now;
        Recovery.GameData.Instance.date = data.Year + "-" + data.Month + "-" + data.Day + " " + data.Hour + ":" + data.Minute;
        Debug.Log(Recovery.GameData.Instance.date);

        for (int i = 0; i < 5; i++)
        {
            MySocket.Instance.listSendValue.Add("REQ:" +Recovery.GameData.Instance.Number+ ";");
        }
        MySocket.Instance.listSendValue.Add("2");
    }

    /// <summary>
    /// 倒计时
    /// </summary>
    public void CountDown()
    {
        if (Recovery.GameData.Instance.isPlayGame && !Recovery.GameData.Instance.isStopGame)
        {
            if (float.Parse(Recovery.GameData.Instance.duration) > 0)
            {
                float fill = (Recovery.GameData.Instance.gameTime * 60 - Recovery.GameData.Instance.currentGameTime) / (Recovery.GameData.Instance.gameTime * 60);
                UIUpdateManaget.Instance.Fill(fill);
                if (Recovery.GameData.Instance.currentGameTime < Recovery.GameData.Instance.gameTime * 60)
                {
                    Recovery.GameData.Instance.currentGameTime += Time.deltaTime;
                }
                else
                {
                    Debug.Log("游戏结束");
                    GameOver();
                }
            }
        }
    }


    /// <summary>
    ///创建物品
    /// </summary>
    /// <param name="vec3Pos"></param>
    /// <param name="source"></param>
    public GameObject EstablishProp(string source)
    {
        var obj = Instantiate(Resources.Load(source)) as GameObject;
        obj.transform.localScale = Vector3.one;
        return obj;
    }
    public GameObject EstablishProp(string source, Vector3 vec3Pos)
    {
        var obj = Instantiate(Resources.Load(source), vec3Pos, Quaternion.identity) as GameObject;
        obj.transform.localScale = Vector3.one;
        return obj;
    }
    public GameObject EstablishProp(string source, Vector3 vec3Pos, Vector3 angle)
    {
        var obj = Instantiate(Resources.Load(source), vec3Pos, Quaternion.identity) as GameObject;
        obj.transform.localEulerAngles = angle;
        obj.transform.localScale = Vector3.one;
        return obj;
    }
    public GameObject EstablishProp(string source, Vector3 vec3Pos, Vector3 angle, Vector3 scale)
    {
        var obj = Instantiate(Resources.Load(source), vec3Pos, Quaternion.identity) as GameObject;
        obj.transform.localEulerAngles = angle;
        obj.transform.localScale = scale;
        return obj;
    }

    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="value"></param>
    public void AddPlayer(BikeZCInfo bikeZCInfo)
    {
        Vector3[] pos = { new Vector3(-1.417f, 0, 0), new Vector3(-0.48f, 0, 0), new Vector3(0.48f, 0, 0), new Vector3(1.417f, 0, 0) };
        GameObject _obj = null;
        if (Recovery.GameData.Instance.gameModeType != Recovery.Type.GameModeType.many)
        {
            queue.Enqueue("Play");
            int i = int.Parse(bikeZCInfo.numId);
            if (i > 4)
                i = 4;
            _obj = EstablishProp(Source.Role.Bicycle + i);
            _obj.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            switch (bikeZCInfo.numId)
            {
                case "0001":
                    _obj = Instantiate(Resources.Load(Source.Role.Bicycle + (1))) as GameObject;
                    _obj.transform.position = pos[0];
                    break;
                case "0002":
                    _obj = Instantiate(Resources.Load(Source.Role.Bicycle + (2))) as GameObject;
                    _obj.transform.position = pos[1];
                    break;
                case "0003":
                    _obj = Instantiate(Resources.Load(Source.Role.Bicycle + (3))) as GameObject;
                    _obj.transform.position = pos[2];
                    break;
                case "0004":
                    _obj = Instantiate(Resources.Load(Source.Role.Bicycle + (4))) as GameObject;
                    _obj.transform.position = pos[3];
                    break;
            }
        }
        if (bikeZCInfo.numId == Recovery.GameData.Instance.Number)
        {
            UIUpdateManaget.Instance.WaitingConnection.gameObject.SetActive(false);
            Recovery.GameData.Instance.Dic_RoleCount.Add(bikeZCInfo.numId, _obj.AddComponent<MainPlayer>());
        }
        else
            Recovery.GameData.Instance.Dic_RoleCount.Add(bikeZCInfo.numId, _obj.AddComponent<_Player>());
        Recovery.GameData.Instance.Dic_RoleCount[bikeZCInfo.numId].InitData(_obj.transform.position, "自行车", bikeZCInfo.numId, false, bikeZCInfo.no, bikeZCInfo._name, bikeZCInfo.sex, bikeZCInfo.age);
        if (Recovery.GameData.Instance.Number == "0001" && Recovery.GameData.Instance.Dic_RoleCount.Count > 1)
            UIUpdateManaget.Instance.playGame.gameObject.SetActive(true);

    }


    /// <summary>
    /// 难度换算
    /// </summary>
    public void DifficultyConversion()
    {
        float _length =(0.08F*2*3.1415f)*Recovery.GameData.Instance.gameTime*60*6;
        if (Recovery.GameData.Instance.gameTime == 0)//无限模式
        {
            switch (Recovery.GameData.Instance.level)
            {
                case "0":
                    break;
                case "1":
                    SetSlopeProp(_length / 5, "15");
                    SetSlopeProp(_length / 5 * 2, "15");
                    SetSlopeProp(_length / 5 * 3, "30");
                    SetSlopeProp(_length / 5 * 4, "30");
                    break;
                case "2":
                    SetSlopeProp(_length / 6, "15");
                    SetSlopeProp(_length / 6 * 2, "30");
                    SetSlopeProp(_length / 6 * 3, "30");
                    SetSlopeProp(_length / 6 * 4, "45");
                    SetSlopeProp(_length / 6 * 5, "45");
                    break;
            }
        }
        if (Recovery.GameData.Instance.gameTime == 5)
        {
            switch (Recovery.GameData.Instance.level)
            {
                case "0":
                    break;
                case "1":
                    SetSlopeProp(_length / 2, "30");
                    break;
                case "2":
                    SetSlopeProp(_length / 2, "45");
                    break;
            }
        }
        if (Recovery.GameData.Instance.gameTime == 10)
        {
            switch (Recovery.GameData.Instance.level)
            {
                case "0":
                    break;
                case "1":
                    SetSlopeProp(_length / 4, "15");
                    SetSlopeProp(_length / 4 * 2, "15");
                    SetSlopeProp(_length / 4 * 3, "30");
                    break;
                case "2":
                    SetSlopeProp(_length / 4, "30");
                    SetSlopeProp(_length / 4 * 2, "30");
                    SetSlopeProp(_length / 4 * 3, "45");
                    break;
            }
        }
        if (Recovery.GameData.Instance.gameTime == 20)
        {
            switch (Recovery.GameData.Instance.level)
            {
                case "0":
                    break;
                case "1":
                    SetSlopeProp(_length / 5, "15");
                    SetSlopeProp(_length / 5 * 2, "15");
                    SetSlopeProp(_length / 5 * 3, "30");
                    SetSlopeProp(_length / 5 * 4, "30");
                    break;
                case "2":
                    SetSlopeProp(_length / 6, "15");
                    SetSlopeProp(_length / 6 * 2, "30");
                    SetSlopeProp(_length / 6 * 3, "30");
                    SetSlopeProp(_length / 6 * 4, "45");
                    SetSlopeProp(_length / 6 * 5, "45");
                    break;
            }
        }
    }

    /// <summary>
    /// 生成坡的方法
    /// </summary>
    /// <param name="timeGame"></param>
    /// <param name=""></param>
    public void SetSlopeProp(float distance, string slopeTyep)
    {
        GameObject obj = null;
        switch (slopeTyep)
        {
            case "15":
                obj = EstablishProp(Source.Prop.slope15, Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f));
                break;
            case "30":
                obj = EstablishProp(Source.Prop.slope30, Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f));
                break;
            case "45":
                obj = EstablishProp(Source.Prop.slope45, Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f));
                break;
        }
        obj.transform.position = new Vector3(0, 0, distance);
        Recovery.GameData.Instance.allSlope.Add(obj);
    }



    /// <summary>
    /// 队列
    /// </summary>
    public Queue<object> queue = new Queue<object>();
    /// <summary>
    /// 队列调用
    /// </summary>
    public void QueuePlay()
    {
        lock (queue)
        {
            if (queue.Count > 0)
            {
                object _obj = queue.Dequeue();
                if (_obj is BikeZCInfo)
                {
                    AddPlayer(_obj as BikeZCInfo);
                }
                if (_obj is string)
                {
                    if (_obj.ToString() == "Play")
                    {
                        UIUpdateManaget.Instance.timeDome.gameObject.SetActive(true);
                        UIUpdateManaget.Instance.timeDome.OnPlay();
                        if (Recovery.GameData.Instance.gameModeType == Recovery.Type.GameModeType.many)
                            UIUpdateManaget.Instance.M_Init_Color();
                    }
                    if (_obj.ToString() == "RPT")
                        Http_WWW.Instance.UploadInfo(Recovery.GameData.Instance.GhipData);
                    if (_obj.ToString() == "DAT")
                    {
                        UIUpdateManaget.Instance.S_Init_InfoData();
                    }
                    if (_obj.ToString() == "GDAT")
                    {
                        UIUpdateManaget.Instance.M__Init_InfoData();
                    }
                    if (_obj.ToString() == "SET")
                        DifficultyConversion();
                    if (_obj.ToString() == "CloentClose")
                    {
                        Recovery.GameData.Instance.isPlayGame = false;
                        UIUpdateManaget.Instance.connectionFailed.gameObject.SetActive(true);
                        UIUpdateManaget.Instance.connectionFailed.OnPlay();
                    }
                    if (_obj.ToString() == "BikeClose")
                    {
                        Recovery.GameData.Instance.isPlayGame = false;
                        UIUpdateManaget.Instance.ConnectionException.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

}
