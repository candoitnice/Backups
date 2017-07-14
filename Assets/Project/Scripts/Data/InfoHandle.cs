using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfoHandle
{
    private static InfoHandle instance;
    public static InfoHandle Instance { get { if (instance == null) instance = new InfoHandle(); return instance; } }

    public Queue<GameDataPacketBike> gameDataPacketBikeQueue=new Queue<GameDataPacketBike>();

    /// <summary>
    /// 初始化事件
    /// </summary>
    public void InitEvent()
    {
        EventManager.Instance.OnREG += Instance_OnREG;
        EventManager.Instance.OnDAT += Instance_OnDAT;
        EventManager.Instance.OnRPT += Instance_OnRPT;
        EventManager.Instance.OnCOD += Instance_OnCOD;
        EventManager.Instance.onGameZCData += Instance_onGameZCData;
        EventManager.Instance.OnGameData += Instance_OnGameData;
        EventManager.Instance.OnGameSET += Instance_OnGameSET;
        EventManager.Instance.OnDisconnect += Instance_OnDisconnect;
        EventManager.Instance.onGameUpdate += GetGameData;
    }

    public void GetGameData()
    {
        if (gameDataPacketBikeQueue.Count > 0)
        {
            EventManager.Instance._OnGameData(gameDataPacketBikeQueue.Dequeue());
        }
    }


    //断开连接
    private void Instance_OnDisconnect(GameDataPacketBike value)
    {
        try
        {
            GameDataPacketBike GDP = value;

            for (int i = 0; i < Recovery.GameData.Instance.Dic_RoleCount.Count; i++)
            {
                Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed = 0;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        GameManager.instance.queue.Enqueue("GDAT");
    }
    //游戏设置
    private void Instance_OnGameSET(HttpInfo value)
    {
        try
        {
            HttpInfo varConfig = value;

            Recovery.GameData.Instance.no = varConfig.no;
            Recovery.GameData.Instance._name = varConfig.name;
            Recovery.GameData.Instance.age = varConfig.age;
            Recovery.GameData.Instance.sex = varConfig.sex;
            Recovery.GameData.Instance.duration = varConfig.config[0].value;
            Recovery.GameData.Instance.mode = varConfig.config[1].value;
            Recovery.GameData.Instance.level = varConfig.config[2].value;
            Recovery.GameData.Instance.gameTime = int.Parse(varConfig.config[0].value);

            GameManager.instance.queue.Enqueue("SET");
        }
        catch (Exception ex)
        {
            Debug.Log("游戏设置" + ex.Message);
        }
    }
    //游戏数据
    private void Instance_OnGameData(GameDataPacketBike value)
    {
        try
        {
            GameDataPacketBike GDP = value;
            if (Recovery.GameData.Instance.Dic_RoleCount.ContainsKey(GDP.bike.numId))
            {
                RoleBase role = Recovery.GameData.Instance.Dic_RoleCount[GDP.bike.numId];
                role.motorSpeed = GDP.bike.speed;
                role.slope = GDP.bike.deg;
                role.heartRate = GDP.bike.heartBeat;
                role.L_symmetry = GDP.bike.offset;
                role.pos = GDP.Pos.GetValue();
                role.distance = GDP.bike.distance;
                role.speedmax = GDP.bike.speedmax;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        GameManager.instance.queue.Enqueue("GDAT");
    }
    //注册数据
    private void Instance_onGameZCData(BikeZCInfo value)
    {
        //Debug.Log("注册数据" + value.ToString());
        try
        {
            BikeZCInfo bikeZC = value;

            if (!Recovery.GameData.Instance.Dic_RoleCount.ContainsKey(bikeZC.numId))
            {
                GameManager.instance.queue.Enqueue(bikeZC);
            }
            else
            {
                RoleBase role = Recovery.GameData.Instance.Dic_RoleCount[bikeZC.numId];

                role.no = bikeZC.no;
                role._name = bikeZC._name;
                role.age = bikeZC.age;
                role.sex = bikeZC.sex;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }


    //状态数据
    private void Instance_OnCOD(string[] value)
    {
        Debug.Log(value[1]);
    }
    //报告数据
    private void Instance_OnRPT(string[] value)
    {
        try
        {
            Recovery.GameData.Instance.energy = value[2];
            Recovery.GameData.Instance.heartbeat = value[3];
            Recovery.GameData.Instance.balance = value[4];
            Recovery.GameData.Instance.speed = value[5];
            Recovery.GameData.Instance.tensionmax = value[6];
            Recovery.GameData.Instance.tensionmin = value[7];
            Recovery.GameData.Instance.tensionavg = value[8];


            Recovery.GameData.Instance.speedavg = (Recovery.GameData.Instance.distance / (Recovery.GameData.Instance.currentGameTime / 60 / 60) / 1000).ToString();
            Debug.Log("平均速度" + Recovery.GameData.Instance.speedavg + "公里/小时");

            GameHttpInfoPacket Ghip = new GameHttpInfoPacket();
            Ghip.bike.no = Recovery.GameData.Instance.no;
            Ghip.bike.name = Recovery.GameData.Instance._name;
            Ghip.bike.age = Recovery.GameData.Instance.age;
            Ghip.bike.sex = Recovery.GameData.Instance.sex;
            //Ghip.bike.gameid = GameData.Instance.gameid;
            //Ghip.bike.gamename = GameData.Instance.gamename;
            Ghip.bike.date = Recovery.GameData.Instance.date;
            Ghip.bike.mode = Recovery.GameData.Instance.mode;
            Ghip.bike.level = Recovery.GameData.Instance.level;
            Ghip.bike.duration = Recovery.GameData.Instance.duration;
            Ghip.bike.distance = Recovery.GameData.Instance.distance.ToString();
            Ghip.bike.energy = Recovery.GameData.Instance.energy;
            Ghip.bike.heartbeat = Recovery.GameData.Instance.heartbeat;
            Ghip.bike.balance = Recovery.GameData.Instance.balance;
            Ghip.bike.spasm = Recovery.GameData.Instance.speed;
            Ghip.bike.tensionmax = Recovery.GameData.Instance.tensionmax;
            Ghip.bike.tensionmin = Recovery.GameData.Instance.tensionmin;
            Ghip.bike.tensionavg = Recovery.GameData.Instance.tensionavg;
            Ghip.bike.speedmax = Recovery.GameData.Instance.speedmax;
            Ghip.bike.speedmin = Recovery.GameData.Instance.speedmin;
            Ghip.bike.speedavg = Recovery.GameData.Instance.speedavg;
            Ghip.bike.speeddata = Recovery.GameData.Instance.speeddata;
            Ghip.bike.angledata = Recovery.GameData.Instance.angledata;
            if (Recovery.GameData.Instance.GhipData == null)
            {
                Recovery.GameData.Instance.GhipData = JsonConvert.SerializeObject(Ghip);
                GameManager.instance.queue.Enqueue("RPT");
            }
            //Debug.Log(str);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    //训练数据
    private void Instance_OnDAT(string[] value)
    {
        try
        {

            Recovery.GameData.Instance.mainPlayer.distance += (0.08f * 2 * 3.1415f) * (float.Parse(value[2]) / 60 / 4);
            Recovery.GameData.Instance.mainPlayer.speedmax = ((0.08f * 2 * 3.1415f) * float.Parse(value[2]) * 60 / 1000);

            Recovery.GameData.Instance.mainPlayer.motorSpeed = float.Parse(value[2]);
            Recovery.GameData.Instance.mainPlayer.slope = int.Parse(value[3]);
            Recovery.GameData.Instance.mainPlayer.heartRate = float.Parse(value[4]);
            Recovery.GameData.Instance.mainPlayer.L_symmetry = float.Parse(value[5].Split(',')[0]);


            GameDataPacketBike trainData = new GameDataPacketBike();
            trainData.bike.numId = value[1];
            trainData.bike.speed = Recovery.GameData.Instance.mainPlayer.motorSpeed;
            trainData.bike.deg = Recovery.GameData.Instance.mainPlayer.slope;
            trainData.bike.heartBeat = Recovery.GameData.Instance.mainPlayer.heartRate;
            trainData.bike.offset = Recovery.GameData.Instance.mainPlayer.L_symmetry;
            trainData.bike.distance = Recovery.GameData.Instance.mainPlayer.distance;
            trainData.bike.speedmax = Recovery.GameData.Instance.mainPlayer.speedmax;
            trainData.Pos.SetValue(Recovery.GameData.Instance.mainPlayer.pos);


            //GameManager.instance.queue.Enqueue(GameData.Instance.mainPlayer);

            if (Recovery.GameData.Instance.gameModeType == Recovery.Type.GameModeType.many)
            {
                Parameter p = new Parameter();
                p.OperaCode = (byte)Operation.GameSyn;
                p.Parameters = new Dictionary<byte, object>();
                ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.GDATA);
                ParameterTool.AddParmerer(p.Parameters, SubCode.GDATA, trainData);
                ClientEngine.Instance.OnSendParamer(p);
            }
            else
                GameManager.instance.queue.Enqueue("DAT");
            GetDat(value);

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    //注册数据
    private void Instance_OnREG(string[] value)
    {
        if (Recovery.GameData.Instance.Number == null)
            Recovery.GameData.Instance.Number = value[1];
        try
        {
            for (int i = 0; i < 5; i++)
            {
                MySocket.Instance.listSendValue.Add("COD:" + value[1] + ":02" + ";");
            }
            MySocket.Instance.listSendValue.Add("2");

            BikeZCInfo bikeZC = new BikeZCInfo();
            bikeZC.no = Recovery.GameData.Instance.no;
            bikeZC._name = Recovery.GameData.Instance._name;
            bikeZC.age = Recovery.GameData.Instance.age;
            bikeZC.sex = Recovery.GameData.Instance.sex;
            bikeZC.numId = value[1];

            if (Recovery.GameData.Instance.gameModeType == Recovery.Type.GameModeType.many)
            {
                Parameter p = new Parameter();
                p.OperaCode = (byte)Operation.AddUser;
                p.Parameters = new Dictionary<byte, object>();
                ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.ZC);
                ParameterTool.AddParmerer(p.Parameters, SubCode.ZC, bikeZC);
                ClientEngine.Instance.OnSendParamer(p);

            }
            //else
            //{
            GameManager.instance.queue.Enqueue(bikeZC);
            //}
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    ///换算报告数据
    /// </summary>
    /// <param name="value"></param>
    public void GetDat(string[] value)
    {
        if (Recovery.GameData.Instance.isPlayGame && !Recovery.GameData.Instance.isStopGame)
        {
            Recovery.GameData.Instance.distance += (0.08f * 2 * 3.1415f) * (float.Parse(value[2]) / 60 / 4);
            float speedmax = ((0.08f * 2 * 3.1415f) * float.Parse(value[2]) * 60 / 1000);
            GetSpeedData(speedmax);
            GetAngleData(float.Parse(value[3]));
            if (float.Parse(Recovery.GameData.Instance.speedmax) < speedmax)
            {
                Recovery.GameData.Instance.speedmax = speedmax.ToString();
                //Debug.Log("最高速度" + GameData.Instance.speedmax + "公里 / 小时");
            }
            if (float.Parse(Recovery.GameData.Instance.speedmin) > speedmax)
            {
                Recovery.GameData.Instance.speedmin = speedmax.ToString();
                //Debug.Log("最低速度" + GameData.Instance.speedmin + "公里 / 小时");
            }
        }
    }

    List<float> listSpeedData = new List<float>();
    private void GetSpeedData(float value)
    {
        listSpeedData.Add(value);
        if (listSpeedData.Count >= 30 * 4)
        {
            float _value = 0;
            for (int i = 0; i < listSpeedData.Count; i++)
            {
                _value += listSpeedData[i];
            }
            Recovery.GameData.Instance.speeddata += ((_value / (30 * 4)).ToString("N2") + ",");
            //Debug.Log(GameData.Instance.speeddata);
            listSpeedData.Clear();
        }
    }

    List<float> listAngleData = new List<float>();
    private void GetAngleData(float value)
    {
        listAngleData.Add(value);
        if (listAngleData.Count >= 30 * 4)
        {
            Recovery.GameData.Instance.angledata += (listAngleData[listAngleData.Count - 1].ToString() + ",");
            //Debug.Log(GameData.Instance.angledata);
            listAngleData.Clear();
        }
    }

    //关闭事件
    public void CloseEvent()
    {
        EventManager.Instance.OnREG -= Instance_OnREG;
        EventManager.Instance.OnDAT -= Instance_OnDAT;
        EventManager.Instance.OnRPT -= Instance_OnRPT;
        EventManager.Instance.OnCOD -= Instance_OnCOD;
        EventManager.Instance.onGameZCData -= Instance_onGameZCData;
        EventManager.Instance.OnGameData -= Instance_OnGameData;
        EventManager.Instance.OnDisconnect -= Instance_OnDisconnect;
        EventManager.Instance.onGameUpdate -= GetGameData;
    }
}