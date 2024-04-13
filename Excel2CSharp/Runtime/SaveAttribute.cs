using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM
{

    [AttributeUsage(AttributeTargets.Field,AllowMultiple = false, Inherited =false)]
    public class ModelSaveIgnoreAttribute : Attribute
    {

        public ModelSaveIgnoreAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Field,AllowMultiple = false, Inherited =false)]
    public class SaveAttribute : Attribute
    {
        /// <summary>
        /// 保存名称
        /// </summary>
        public string SaveName { get; private set; }
        public SaveAttribute(string saveName)
        {
            this.SaveName = saveName;
        }
    }
}