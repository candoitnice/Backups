using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class InfoData
    {
        public InfoType DType = InfoType.Null;
        public object msg { get; set; }

    }

    public enum InfoType
    {
        Null,
        ZC,
        GPLAY,
        GDATA,
        BDATA,
        DKLJ,
        SET
    }

    public class Vector
    {
        public float x = 0f;
        public float y = 0f;
        public float z = 0f;

        public Vector() { }
        public void SetValue(Vector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public Vector3 GetValue()
        {
            return new Vector3(x, y, z);
        }
    }






    /// <summary>
    /// 注册信息
    /// </summary>
    public class BikeZCInfo
    {
        /// <summary>
        /// 病历号
        /// </summary>
        public string no = "11111111";
        /// <summary>
        /// 姓名
        /// </summary>
        public string _name = "张三";
        /// <summary>
        /// 年龄
        /// </summary>
        public string age = "30";
        /// <summary>
        /// 性别
        /// </summary>
        public string sex = "M";
        /// <summary>
        /// 编号
        /// </summary>
        public string numId = "";
    }



    /// <summary>
    /// 游戏训练数据包
    /// </summary>
    public class GameDataPacketBike
{
        public BikeInfo bike = new BikeInfo();
        public Vector Pos = new Vector();
    }
    /// <summary>
    /// 自行车训练数据
    /// </summary>
    public class BikeInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string numId = "";
        /// <summary>
        /// 转速
        /// </summary>
        public float speed = 0f;
        /// <summary>
        /// 坡度
        /// </summary>
        public float deg = 0;
        /// <summary>
        ///心率 
        /// </summary>
        public float heartBeat = 0f;
        /// <summary>
        ///对称性
        /// </summary>
        public float offset = 0;
        /// <summary>
        ///距离
        /// </summary>
        public float distance = 0;

    /// <summary>
    ///距离
    /// </summary>
    public int balluteCount = 0;
    /// <summary>
    ///时速
    /// </summary>
    public float speedmax=0;

    public float balloonCapacity;
    public int nowCount;
}




    /// <summary>
    /// 游戏数据包
    /// </summary>
    public class GameHttpInfoPacket
    {
        public HttpData bike = new HttpData();
    }
    /// <summary>
    /// 
    /// </summary>
    public class HttpData
    {
        public string no = "201701010001";
        public string name = " 张三甲";
        public string age = " 30";
        public string sex = "M";
        public string gameid = "balloon";
        public string gamename = "打气球训练";
        public string date = "2017-04-01 08=00";
        public string mode = "主动模式";
        public string level = "中等";
        public string duration = " 600";
        public string distance = "200";
        public string energy = "1200";
        public string heartbeat = " 75";
        public string balance = " 45;55";
        public string spasm = " 0";
        public string tensionmax = "20";
        public string tensionmin = "12";
        public string tensionavg = "13";
        public string speedmax = "7.9";
        public string speedmin = " 4";
        public string speedavg = "6.35";
        public string speeddata = "4,4.8,7,7.9,7.1,6.5,6.8,5.6,5.1,4.9";
        public string angledata = "0,1,0,0,2,1,1,0,2,0";
    }



    /// <summary>
    /// Http数据
    /// </summary>
    public class HttpInfo
    {
        public string no = "";
        public string name = "";
        public string age = "";
        public string sex = "";
        public IDInfo[] config = new IDInfo[3];
    }
    public class IDInfo
    {
        public string id = "";
        public string name = "";
        public string value = "";
    }






/// <summary>
/// 设置数据
/// </summary>
public class SetGame
{
    //{"训练机通讯":{"IP":"192.168.0.147","端口":"9000"},"联机通讯服务器":{"IP":"192.168.0.147","端口":"6000"},"是否主机":false}
    public bool 是否主机 = false;
    public string GameName = "康复游戏";
    public Bike 训练机通讯 = new Bike();
    public Bike 联机通讯服务器 = new Bike();
    public Platform 平台 = new Platform();
    //public string 主机编号 ="0001";
}
public class Bike
{
    public string IP = "127.0.0.1";
    public string 端口 = "9000";
}
public class Platform
{
    public string Get = "http://127.0.0.1:8080/api/game/start";
    public string Set = "http://127.0.0.1:8080/api/game/end";
}