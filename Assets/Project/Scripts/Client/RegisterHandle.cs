using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Project.Scripts.Client
{
    class RegisterHandle : HandleBase
    {
        public override Operation OpCode
        {
            get
            {
                return Operation.Register;
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
                case SubCode.GameType:
                    UIUpdateManaget.Instance.GameExecutingNotice.gameObject.SetActive(true);
                    Recovery.GameData.Instance.isGameAgain = true;
                    UIUpdateManaget.Instance.GameExecutingNotice.OnPlay();
                    //Debug.Log(ParameterTool.GetParmerer<GameType>(Parma.Parameters, subcode));
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
