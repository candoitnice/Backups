using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Recovery
{
    public class Type
    {
        public enum GameModeType
        {
            single,
            automatic,
            many
        }
        public enum GameLevelType
        {
            simple,
            secondary,
            difficulty
        }
        public enum GameTimeType
        {
            _15,
            _30,
            _45
        }
        public enum GameEditionType
        {
            children,
            Adult
        }
        public enum GameBalluteType
        {
            green,
            blue,
            red,
            violet
        }
        public enum MachineMode
        {
            Up = 0,
            Down = 1
        }
    }
}