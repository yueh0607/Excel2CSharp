using System;

namespace FFramework.MVVM.Config
{

    /// <summary>
    /// 用于标记主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : System.Attribute
    {

    }
}