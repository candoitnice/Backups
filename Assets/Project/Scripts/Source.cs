public class Source
{
    public class Role
    {
        public static string qiben
        {
            get {
                if (Recovery.Main.Instance.GEType==Recovery.Type.GameEditionType.Adult)
                    return "Prefabs/Role/qiben";
                else
                    return "Prefabs/Role/qiben";
            }
        }
    }
    public class Prop
    {
        public const string slope15 = "Prefabs/slope15";
        public const string slope30 = "Prefabs/slope30";
        public const string slope45 = "Prefabs/slope45";
    }
    public class Effect
    {
        public const string blast = "Prefabs/Effect/Blast";
        public const string Ballute_Blast = "Prefabs/Effect/Ballute_Blast";

    }
}