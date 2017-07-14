using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recovery
{
    public class GameData
    {
        private static GameData instance;
        public static GameData Instance
        {
            get
            {
                if (instance == null) instance = new GameData();
                return instance;
            }
        }

        #region 游戏相关字段
        /// <summary>
        /// 游戏编号
        /// </summary>
        public string Number;
        /// <summary>
        /// 所有玩家
        /// </summary>
        public Dictionary<string, RoleBase> Dic_RoleCount = new Dictionary<string, RoleBase>();
        /// <summary>
        /// 主要的玩家
        /// </summary>
        public MainPlayer mainPlayer;
        /// <summary>
        /// 所有的坡
        /// </summary>
        public List<GameObject> allSlope = new List<GameObject>();
        /// <summary>
        /// 游戏时间
        /// </summary>
        public int gameTime;
        /// <summary>
        /// 当前游戏时间
        /// </summary>
        public float currentGameTime;
        /// <summary>
        /// 是否开始游戏
        /// </summary>
        public bool isPlayGame;
        /// <summary>
        /// 是否暂停游戏
        /// </summary>
        public bool isStopGame;
        /// <summary>
        ///游戏模式
        /// </summary>
        public Recovery.Type.GameModeType gameModeType;
        /// <summary>
        ///游戏难度
        /// </summary>
        public Recovery.Type.GameLevelType gameLevelType;
        /// <summary>
        ///游戏时间
        /// </summary>
        public Recovery.Type.GameTimeType timeType;
        /// <summary>
        /// 
        /// </summary>
        public string GhipData;
        /// <summary>
        /// 当前坡的索引
        /// </summary>
        public int slopeCount;
        #endregion

        #region Json
        /// <summary>
        /// 自行车服务器IP
        /// </summary>
        public string bikeIp;
        /// <summary>
        /// 自行车服务器端口
        /// </summary>
        public string bikePort;
        /// <summary>
        ///通讯服务器IP
        /// </summary>
        public string serverIp;
        /// <summary>
        ///通讯服务器端口
        /// </summary>
        public string serverPort;
        #endregion

        #region HTTP数据
        /// <summary>
        /// 病历号
        /// </summary>
        public string no;
        /// <summary>
        /// 姓名
        /// </summary>
        public string _name;
        /// <summary>
        /// 年龄
        /// </summary>
        public string age;
        /// <summary>
        /// 性别
        /// </summary>
        public string sex;
        /// <summary>
        /// 训练项目
        /// </summary>
        public string gameid;
        /// <summary>
        /// 训练名称
        /// </summary>
        public string gamename;
        /// <summary>
        /// 训练日期
        /// </summary>
        public string date;
        /// <summary>
        /// 训练模式
        /// </summary>
        public string mode;
        /// <summary>
        /// 训练难度
        /// </summary>
        public string level;
        /// <summary>
        /// 训练时间
        /// </summary>
        public string duration;
        /// <summary>
        /// 运动距离
        /// </summary>
        public float distance;
        /// <summary>
        /// 消耗能量
        /// </summary>
        public string energy;
        /// <summary>
        /// 平均心率
        /// </summary>
        public string heartbeat;
        /// <summary>
        /// 对称性
        /// </summary>
        public string balance;
        /// <summary>
        /// 痉挛次数
        /// </summary>
        public string speed;
        /// <summary>
        /// 最大肌张力
        /// </summary>
        public string tensionmax;
        /// <summary>
        /// 最小肌张力
        /// </summary>
        public string tensionmin;
        /// <summary>
        /// 平均肌张力
        /// </summary>
        public string tensionavg;
        /// <summary>
        /// 最高速度
        /// </summary>
        public string speedmax = "0";
        /// <summary>
        /// 最低速度
        /// </summary>
        public string speedmin = "1000000";
        /// <summary>
        /// 平均速度
        /// </summary>
        public string speedavg;
        /// <summary>
        /// 速度数据
        /// </summary>
        public string speeddata;
        /// <summary>
        /// 坡度数据
        /// </summary>
        public string angledata;
        #endregion


    }
}