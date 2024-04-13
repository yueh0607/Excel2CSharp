using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace FFramework.FUnityEditor
{
    public static class EditorHelper
    {
        private static string projectPath = null;
        /// <summary>
        /// 项目所在的路径
        /// </summary>
        public static string ProjectPath
        {
            get
            {
                if (projectPath == null)
                {
                    projectPath = new DirectoryInfo(Application.dataPath).Parent.FullName;
                }
                return projectPath;
            }
        }


        /// <summary>
        /// 获取相对于Asset路径的绝对路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetAbsPathToAsset(string relativePath) => Path.Combine(Application.dataPath, relativePath);
        /// <summary>
        /// 获取相对于项目路径的绝对路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetAbsPathToProject(string relativePath) => Path.Combine(ProjectPath, relativePath);

        /// <summary>
        /// 检验是否是有效的Assets路径
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public static bool IsInvalidProjectDirectory(string projectPath)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(ProjectPath,projectPath));
            //目录存在
            if (!directory.Exists) return false;
            //包含Assets
            //if (!directory.FullName.Contains("Assets")) return false;
            return true;
        }

        /// <summary>
        /// 检验是否是有效的Assets路径
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public static bool IsInvalidAssetsDirectory(string assetsPath)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(Application.dataPath,assetsPath));
            //目录存在
            if (!directory.Exists) return false;
            //包含Assets
            //if (!directory.FullName.Contains("Assets")) return false;

            return true;
        }
        public static bool IsInvalidAbsDirectory(string absPath)
        {
            DirectoryInfo directory = new DirectoryInfo( absPath);
            //目录存在
            if (!directory.Exists) return false;

            return true;
        }

        /// <summary>
        /// 不存在则创建，可以是文件夹也可以是文件
        /// </summary>
        /// <param name="path"></param>
        public static void NotExistCreate(string path)
        {
            if (Path.HasExtension(path))
            {
                string dir = Path.GetDirectoryName(path);
                Directory.CreateDirectory(dir);
                if (!File.Exists(path))
                    File.Create(path).Dispose();
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            if (Path.HasExtension(path))
            {
                if (File.Exists(path)) File.Delete(path);
                return;
            }

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
        /// <summary>
        /// 深度遍历
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void ForeachDFS(string path, Action<string> action, Predicate<string> filter = null)
        {
            foreach (var dir in Directory.EnumerateDirectories(path))
            {
                ForeachDFS(dir, action, filter);
            }
            foreach (var files in Directory.EnumerateFiles(path))
            {
                if (filter == null || filter(files))
                    action(files);
            }
        }

        /// <summary>
        /// 深度遍历
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void ForeachBFS(string path, Action<string> action, Predicate<string> filter = null)
        {
            foreach (var files in Directory.EnumerateFiles(path))
            {
                if (filter == null || filter(files))
                    action(files);
            }
            foreach (var dir in Directory.EnumerateDirectories(path))
            {
                ForeachDFS(dir, action, filter);
            }
        }

        /// <summary>
        /// 向Unity编辑器添加宏，不会重复添加
        /// </summary>
        /// <param name="buildTargetGroup"></param>
        /// <param name="macro"></param>
        public static void AddMacro(BuildTargetGroup buildTargetGroup, string macro)
        {
            string tmpSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            List<string> marcos = tmpSymbols.Split(';').ToList();

            if (!marcos.Contains(macro))
            {
                marcos.Add(macro);
                tmpSymbols = string.Join(";", marcos.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, tmpSymbols);
            }
        }

        /// <summary>
        /// 从Unity编辑器移除宏
        /// </summary>
        /// <param name="buildTargetGroup"></param>
        /// <param name="macro"></param>
        public static void RemoveMacro(BuildTargetGroup buildTargetGroup, string macro)
        {
            string tmpSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            List<string> marcos = tmpSymbols.Split(';').ToList();

            if (marcos.Contains(macro))
            {
                marcos.Remove(macro);
                tmpSymbols = string.Join(";", marcos.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, tmpSymbols);
            }
        }
        /// <summary>
        /// 获取宏
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMarcos(BuildTargetGroup buildTargetGroup)
        {
            string tmpSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            List<string> marcos = tmpSymbols.Split(';').ToList();
            return marcos;
        }

        /// <summary>
        /// 需要编译
        /// </summary>
        /// <param name="onCompleted"></param>
        public static void NeefReCompile(Action<object> onCompleted = null)
        {
            UnityEditor.Compilation.CompilationPipeline.compilationFinished += onCompleted;
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }

        static EditorHelper()
        {
            UnityEditor.Compilation.CompilationPipeline.compilationStarted += (x) => compiling = true;
            UnityEditor.Compilation.CompilationPipeline.compilationFinished += (x) => compiling = false;
        }
        //是否正在编译
        public static bool compiling = false;

    }
}