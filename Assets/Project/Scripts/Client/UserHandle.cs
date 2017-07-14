using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Sport
{
    class UserHandle : HandleBase
    {
        public override Operation OpCode
        {
            get
            {
                return Operation.AddUser;
            }
        }

        public delegate void HandleEventMachineRegester(Machine m);

        public override void Handle(Parameter dic)
        {
            SubCode subcode = ParameterTool.GetParmerer<SubCode>(dic.Parameters, PameraCode.SubCode);
            switch (subcode)
            {
                case SubCode.ZC:
                    //Debug.Log(ParameterTool.GetParmerer<BikeZCInfo>(dic.Parameters, subcode));
                    EventManager.Instance._OnGameZCData(ParameterTool.GetParmerer<BikeZCInfo>(dic.Parameters, SubCode.ZC));
                    break;
            }
        }

        public void Register(Machine machine)
        {
            Console.WriteLine(machine.ID);
            Parameter p = new Parameter();
            p.OperaCode = (byte)OpCode;
            p.Parameters = new Dictionary<byte, object>();
            ParameterTool.AddParmerer(p.Parameters, PameraCode.SubCode, SubCode.Null);
            ParameterTool.AddParmerer(p.Parameters, SubCode.DKLJ, machine);

        }
    }
}
