public class Source
{
    public class Role
    {
        public static string Bicycle
        {
            get {
                if (Main.Instance.GEType==Recovery.Type.GameEditionType.Adult)
                    return "Prefabs/Role/Bicycle";
                else
                    return "Prefabs/Role/X_Bicycle";
            }
        }
    }
    public class Prop
    {
        public const string slope15 = "Prefabs/slope15";
        public const string slope30 = "Prefabs/slope30";
        public const string slope45 = "Prefabs/slope45";
    }
}