using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Recovery
{
    public class InfoHandle
    {
        private static InfoHandle instance;
        public static InfoHandle Instance { get { if (instance == null) instance = new InfoHandle(); return instance; } }

        public Queue<GameDataPacketBike> gameDataPacketBikeQueue = new Queue<GameDataPacketBike>();

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
        private void Instance_OnDisconnect(BikeZCInfo value)
        {
            try
            {
                Debug.Log(value.numId);
                if (Recovery.GameData.Instance.Dic_RoleCount.ContainsKey(value.numId))
                {
                    RoleBase role = Recovery.GameData.Instance.Dic_RoleCount[value.numId];
                    role.Off_line();
                    UIUpdateManaget.Instance.Disconnect(value.numId);
                    Recovery.GameData.Instance.Dic_RoleCount.Remove(value.numId);
                    Debug.Log(value.numId + "用户掉线");
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
                    role.SetInfo(GDP.bike.speed, GDP.bike.deg, GDP.bike.heartBeat, GDP.bike.offset, GDP.bike.balloonCapacity);
                    role.nowCount = GDP.bike.nowCount;
                    //role.pos = GDP.Pos.GetValue();
                    role.balluteCount = GDP.bike.balluteCount;
                    //role.distance  = GDP.bike.distance;
                    //role.speedmax = GDP.bike.speedmax;
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
        }
        //注册数据
        private void Instance_onGameZCData(List<BikeZCInfo> value)
        {
            //Debug.Log("注册数据" + value.ToString());
            try
            {
                for (int i = 0; i < value.Count; i++)
                {
                    BikeZCInfo bikeZC = value[i];

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
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }


        //状态数据
        private void Instance_OnCOD(string[] value)
        {
            Debug.Log(value[2]);
            switch (value[2])
            {
                case "00":
                    Recovery.GameManager.instance.queue.Enqueue("BikeClose");
                    break;
                case "01":
                    Recovery.GameManager.instance.queue.Enqueue("BikeClose");
                    break;
                case "02":
                    Recovery.GameManager.instance.queue.Enqueue("BikeClose");
                    break;
            }
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


                HttpData hd = new HttpData();
                hd.no = Recovery.GameData.Instance.no;
                hd.name = Recovery.GameData.Instance._name;
                hd.age = Recovery.GameData.Instance.age;
                hd.sex = Recovery.GameData.Instance.sex;
                hd.gameid = GameData.Instance.gameid;
                hd.gamename = GameData.Instance.gamename;
                hd.date = Recovery.GameData.Instance.date;
                /*
                 * 0=主动模式／1=被动模式／2=比赛模式
                 * 0=简单／1=中等／2=困难
                 */

                switch (Recovery.GameData.Instance.mode)
                {
                    case "0":
                        hd.mode = "主动模式";
                        break;
                    case "1":
                        hd.mode = "被动模式";
                        break;
                    case "2":
                        hd.mode = "比赛模式";
                        break;
                }
                switch (Recovery.GameData.Instance.level)
                {
                    case "0":
                        hd.level = "简单";
                        break;
                    case "1":
                        hd.level = "中等";
                        break;
                    case "2":
                        hd.level = "困难";
                        break;
                }

                hd.duration = ((int)Recovery.GameData.Instance.currentGameTime).ToString();
                hd.distance = Recovery.GameData.Instance.distance.ToString();
                hd.energy = Recovery.GameData.Instance.energy;
                hd.heartbeat = Recovery.GameData.Instance.heartbeat;
                hd.balance = Recovery.GameData.Instance.balance;
                hd.spasm = Recovery.GameData.Instance.speed;
                hd.tensionmax = Recovery.GameData.Instance.tensionmax;
                hd.tensionmin = Recovery.GameData.Instance.tensionmin;
                hd.tensionavg = Recovery.GameData.Instance.tensionavg;
                hd.speedmax = Recovery.GameData.Instance.speedmax;
                hd.speedmin = Recovery.GameData.Instance.speedmin;
                hd.speedavg = Recovery.GameData.Instance.speedavg;
                hd.speeddata = Recovery.GameData.Instance.speeddata.Remove(GameData.Instance.speeddata.Length - 1);
                hd.angledata = Recovery.GameData.Instance.angledata.Remove(GameData.Instance.angledata.Length - 1);
                if (Recovery.GameData.Instance.GhipData == null)
                {
                    Recovery.GameData.Instance.GhipData = JsonConvert.SerializeObject(hd);
                    GameManager.instance.queue.Enqueue("RPT");
                }
                //Debug.Log(Recovery.GameData.Instance.GhipData);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            MySocket.Instance.listSendValue.Add("COD:" + value[1] + ":99;");
            if (GameData.Instance.gameModeType == Type.GameModeType.many)
            {
                Parameter p = new Parameter();
                p.OperaCode = (byte)Operation.GameSyn;
                p.Parameters = new Dictionary<byte, object>();
                ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.BDATA);
                ParameterTool.AddParmerer(p.Parameters, SubCode.BDATA, GameData.Instance.GDP_Bike);
                ClientEngine.Instance.OnSendParamer(p);
                GameData.Instance.GDP_Bike = null;
            }
        }
        //训练数据
        private void Instance_OnDAT(string[] value)
        {
            try
            {
              
                //Recovery.GameData.Instance.mainPlayer.distance += (0.08f * 2 * 3.1415f) * (float.Parse(value[2]) / 60 / 4);
                Recovery.GameData.Instance.mainPlayer.speedmax = ((0.08f * 2 * 3.1415f) * float.Parse(value[2]) * 60 / 1000);

                Recovery.GameData.Instance.mainPlayer.motorSpeed = float.Parse(value[2]);
                Recovery.GameData.Instance.mainPlayer.slope = int.Parse(value[3]);
                Recovery.GameData.Instance.mainPlayer.heartRate = float.Parse(value[4]);
                Recovery.GameData.Instance.mainPlayer.L_symmetry = float.Parse(value[5].Split(',')[0]);


                GameDataPacketBike GDP_Bike = new GameDataPacketBike();
                GDP_Bike.bike.numId = value[1];
                GDP_Bike.bike.speed = Recovery.GameData.Instance.mainPlayer.motorSpeed;
                GDP_Bike.bike.deg = Recovery.GameData.Instance.mainPlayer.slope;
                GDP_Bike.bike.heartBeat = Recovery.GameData.Instance.mainPlayer.heartRate;
                GDP_Bike.bike.offset = Recovery.GameData.Instance.mainPlayer.L_symmetry;
                GDP_Bike.bike.distance = Recovery.GameData.Instance.mainPlayer.distance;
                GDP_Bike.bike.balluteCount = Recovery.GameData.Instance.mainPlayer.balluteCount;
                GDP_Bike.bike.speedmax = Recovery.GameData.Instance.mainPlayer.speedmax;
                GDP_Bike.bike.balloonCapacity = Recovery.GameData.Instance.mainPlayer.balloonCapacity;
                GDP_Bike.bike.nowCount= Recovery.GameData.Instance.mainPlayer.nowCount;

                GameData.Instance.GDP_Bike = GDP_Bike;
                GameData.Instance.mainPlayer.SetInfo(GDP_Bike.bike.speed, GDP_Bike.bike.deg, GDP_Bike.bike.heartBeat, GDP_Bike.bike.offset, (0.08f * 2 * 3.1415f) * (GDP_Bike.bike.speed / 60 / 4));

                GameManager.instance.queue.Enqueue("DAT");
                GetDat(value);

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }              
            MySocket.Instance.listSendValue.Add("COD:" + value[1] + ":99;");
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
                for (int i = 0; i < 5; i++)
                {
                    MySocket.Instance.listSendValue.Add("CLR:" + value[1] + ":"+0+":"+int.Parse(GameData.Instance.mode)+":;");
                }
                Debug.Log("CLR:" + value[1] + ":1:" + GameData.Instance.mode + ";");

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
                GetAngleData(float.Parse(value[3]));
                GetSpeedData(speedmax);
               
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

        float currentTime;
        float count;
        List<float> listSpeedData = new List<float>();
        private void GetSpeedData(float value)
        {
            listSpeedData.Add(value);
            if ((int)GameData.Instance.currentGameTime >= currentTime && count < GameData.Instance.gameTime * 60)
            {
                float _value = 0;
                for (int i = 0; i < listSpeedData.Count; i++)
                {
                    _value += listSpeedData[i];
                }
                count++;
                currentTime = (int)GameData.Instance.currentGameTime + 1f;
                Recovery.GameData.Instance.speeddata += ((_value / listSpeedData.Count).ToString("N2") + ",");
                //Debug.Log(GameData.Instance.speeddata);
                listSpeedData.Clear();
            }
        }

        List<float> listAngleData = new List<float>();
        private void GetAngleData(float value)
        {
            listAngleData.Add(value);
            //Debug.Log(listAngleData.Count);

            if ((int)GameData.Instance.currentGameTime >= currentTime && count < GameData.Instance.gameTime * 60)
            {
                Recovery.GameData.Instance.angledata += (listAngleData[listAngleData.Count - 1] + ",");
                //Debug.Log(GameData.Instance.angledata);
                listAngleData.Clear();
            }
        }

        public void InitGameData()
        {
            count = 0;
            currentTime = 0;
            Recovery.GameData.Instance.angledata = "";
            Recovery.GameData.Instance.speeddata = "";
            listAngleData.Clear();
            listSpeedData.Clear();
            Recovery.GameData.Instance.speedmax = "";
            Recovery.GameData.Instance.speedmin = "";
            Recovery.GameData.Instance.distance = 0;
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
}