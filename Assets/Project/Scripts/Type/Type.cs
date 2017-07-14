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
    }
}