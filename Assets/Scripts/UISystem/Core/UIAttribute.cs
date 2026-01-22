using System;

namespace UISystem.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIAttribute : Attribute
    {
        public string ResPath{get;private set;}
        public UIAttribute(string path)
        {
            ResPath = path;
        }
    }   

    public enum UILifeTimeType
    {
        Transient,
        Permanent,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UILifeTime : Attribute
    {
        private UILifeTimeType windowLifeTimeType;
        public bool IsPermanent => windowLifeTimeType == UILifeTimeType.Permanent;
        public UILifeTime(UILifeTimeType inType)
        {
            windowLifeTimeType = inType;
        }
    }
}