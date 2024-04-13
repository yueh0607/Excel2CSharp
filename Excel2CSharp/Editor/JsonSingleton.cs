using System;
using System.IO;
using System.Reflection;
using UnityEditor;

namespace FFramework.FUnityEditor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FilePathAttribute : Attribute
    {
        public enum Location
        {
            ProjectFolder,
            AssetsFolder,
            AbsoluteFoder,
            PerferenceFolder
        }

        string relative;

        /// <summary>
        /// 相对于Project的路径
        /// </summary>
        public string Path => relative;

        /// <summary>
        /// 绝对路径
        /// </summary>
        public string Abs => System.IO.Path.Combine(EditorHelper.ProjectPath, relative);

        public FilePathAttribute(string relativePath, Location location)
        {
            switch (location)
            {
                case Location.ProjectFolder:
                    relative = relativePath;
                    break;
                case Location.AssetsFolder:
                    relative = "Assets/" + relativePath;
                    break;
                case Location.AbsoluteFoder:
                    relative = System.IO.Path.GetRelativePath(EditorHelper.ProjectPath, relativePath);
                    break;
                case Location.PerferenceFolder:
                    relative = System.IO.Path.GetRelativePath(System.IO.Path.Combine(EditorHelper.ProjectPath, "UserSettings/"), relativePath);
                    break;

            }
        }
    }

    /// <summary>
    /// 以Json形式存储的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonSingleton<T> where T : JsonSingleton<T>, new()
    {
        private static T _ins = null;
        public static T instance
        {
            get
            {
                //空
                if (_ins == null)
                {
                    //检查特性
                    FilePathAttribute att = typeof(T).GetCustomAttribute<FilePathAttribute>();
                    _ins = new T();
                    //有特性
                    if (att != null&&File.Exists(att.Abs)) 
                    {
                        
                        //读取并覆盖
                        using (StreamReader reader = new StreamReader(att.Abs))
                        {
                            string json = reader.ReadToEnd();
                            EditorJsonUtility.FromJsonOverwrite(json,_ins);
                        }
                    }
                }
                return _ins;

            }
        }

        public void Save()
        {
            //转json
            string json = EditorJsonUtility.ToJson(this, true);
            //查特性
            FilePathAttribute att = typeof(T).GetCustomAttribute<FilePathAttribute>();
            //无特性抛异常
            if (att == null)
            {
                throw new System.Exception($"{typeof(T).FullName} need {typeof(FilePathAttribute).FullName}");
            }
            else
            {
                EditorHelper.NotExistCreate(att.Abs);
                //有则保存
                using (StreamWriter writer = new StreamWriter(att.Abs, false, System.Text.Encoding.UTF8))
                {
                    writer.Write(json);
                }
            }
        }
    }
}