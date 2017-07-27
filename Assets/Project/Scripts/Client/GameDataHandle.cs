using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Project.Scripts.Client
{
    class GameDataHandle : HandleBase
    {
        public override Operation OpCode
        {
            get
            {
                return Operation.GameSyn;
            }
        }
        /// <summary>
        /// 处理设备消息
        /// </summary>
        /// <param name="Parma"></param>
        public override void Handle(Parameter Parma)
        {
            SubCode subcode = ParameterTool.GetParmerer<SubCode>(Parma.Parameters, PameraCode.SubCode);
            switch (subcode)
            {
                case SubCode.SET:
                    EventManager.Instance._OnGameSET(ParameterTool.GetParmerer<HttpInfo>(Parma.Parameters, subcode));
                    break;
                case SubCode.GPLAY:
                    EventManager.Instance._OnPlayGame();
                    break;
                case SubCode.GDATA:
                    //UIUpdateManaget.Instance.debug.text = ParameterTool.GetParmerer<object>(Parma.Parameters, subcode).ToString();
                    Recovery.InfoHandle.Instance.gameDataPacketBikeQueue.Enqueue(ParameterTool.GetParmerer<GameDataPacketBike>(Parma.Parameters, subcode));
                    break;
                case SubCode.DKLJ:
                    EventManager.Instance._OnDisconnect(ParameterTool.GetParmerer<BikeZCInfo>(Parma.Parameters, subcode));
                    break;
                case SubCode.BDATA:
                    UIUpdateManaget.Instance.SetRankingList();
                    break;
            }
        }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        public void GetEquipInfo()
        {
            Parameter p = new Parameter();
            p.OperaCode = (byte)OpCode;
            p.Parameters = new Dictionary<byte, object>();
            ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.Null);
            ClientEngine.Instance.OnSendParamer(p);
        }
    }

}
