using FFramework.MVVM.UnityEditor;
using FFramework.FUnityEditor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;



namespace FFramework.MVVM.UnityEditor
{

    [FUnityEditor.FilePath("FFramework/Settings/ConfigSettings.json", FUnityEditor.FilePathAttribute.Location.ProjectFolder)]
    public class ConfigSettings : JsonSingleton<ConfigSettings>
    {
        public string generatePath = "Assets/Scripts/Project/ConfigCache/";
        public string originPath = "ExcelConfig/";
        public List<string> ignoreFileName = new List<string>();

        //public void Modify() => Save(true);
    }

    public class ConfigWindow : EditorWindow
    {
        [MenuItem("ConfigTool", menuItem = "FFramework/ConfigTool")]
        static void Open()
        {
            var window = GetWindow<ConfigWindow>();
            window.Show();

        }
        private void OnDisable()
        {
            ConfigSettings.instance.Save();
            //EditorUtility.SetDirty(ConfigSettings.instance);
            //AssetDatabase.SaveAssets();
        }

        bool locked = false;
        Vector2 pos = Vector2.zero;
        private void OnGUI()
        {

            //打开生成路径
            GUILayout.BeginHorizontal();
            ConfigSettings.instance.generatePath = EditorGUILayout.TextField("GeneratePath", ConfigSettings.instance.generatePath, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Open", GUILayout.ExpandWidth(false)))
            {
                Application.OpenURL(Path.Combine(EditorHelper.ProjectPath, ConfigSettings.instance.generatePath));
            }
            GUILayout.EndHorizontal();

            //打开源路径
            GUILayout.BeginHorizontal();
            ConfigSettings.instance.originPath = EditorGUILayout.TextField("OriginPath", ConfigSettings.instance.originPath);
            if (GUILayout.Button("Open", GUILayout.ExpandWidth(false)))
            {
                Application.OpenURL(Path.Combine(EditorHelper.ProjectPath, ConfigSettings.instance.originPath));
            }
            GUILayout.EndHorizontal();


            string path = Path.Combine(EditorHelper.ProjectPath, ConfigSettings.instance.originPath);

            GUILayout.BeginHorizontal();
            //生成全部
            if (GUILayout.Button("Generate All"))
            {
                GenerateAll();
            }
            //重新生成全部
            if (GUILayout.Button("ReMark All"))
            {
                EditorHelper.ForeachDFS(path, (x) =>
                {
                    FileSystemInfo xlsFile = new FileInfo(x);
                    xlsFile.LastWriteTimeUtc = DateTime.UtcNow;

                }, (p) =>
                {
                    string str = Path.GetExtension(p);
                    string relative = Path.GetRelativePath(EditorHelper.ProjectPath, p);
                    //xlsx结尾
                    bool result = (str == ".xlsx" || str == ".xls")
                    //文件名不开头是~
                    && (!Path.GetFileName(p).StartsWith("~"))
                    //不是忽略文件里的
                    && (!ConfigSettings.instance.ignoreFileName.Contains(relative));


                    return result;
                });
            }
            //清除全部
            //重新生成全部
            if (GUILayout.Button("Clear All"))
            {
                foreach (string p in Directory.EnumerateFiles(ConfigSettings.instance.generatePath))
                {
                    File.Delete(p);
                }
                AssetDatabase.Refresh();
            }
            GUILayout.EndHorizontal();

            //Excel文件列表
            pos = GUILayout.BeginScrollView(pos);
            //是否锁定生成
            locked = false;
            //相对路径合成绝对路径

            //存在源路径
            if (Directory.Exists(path))
                //遍历xlsx或xls文件
                EditorHelper.ForeachDFS(path, (x) =>
                {
                    //生成路径
                    string genPath = GetGenPath(x);
                    string relative = Path.GetRelativePath(EditorHelper.ProjectPath, x);
                    //文件信息
                    FileSystemInfo genFile = new FileInfo(genPath);
                    FileSystemInfo xlsFile = new FileInfo(x);
                    //文件是否变动
                    bool changed = xlsFile?.LastWriteTimeUtc != genFile?.LastWriteTimeUtc;
                    bool ignore = ConfigSettings.instance.ignoreFileName.Contains(relative);

                    GUILayout.BeginHorizontal();
                    Color last = GUI.color;
                    //变更则红色
                    GUI.color = changed ? Color.yellow : last;
                    //忽略为蓝色
                    if (ignore)
                    {
                        GUI.color = Color.gray;
                        GUILayout.TextArea(Path.GetFileName(x) + " (Ignored)", GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        GUILayout.TextArea(Path.GetFileName(x), GUILayout.ExpandWidth(true));
                    }
                    GUI.color = last;

                    //打开具体文件
                    if (GUILayout.Button("Open", GUILayout.ExpandWidth(false)))
                    {
                        Application.OpenURL(x);
                    }
                    //跟踪文件
                    if (ignore)
                    {
                        if (GUILayout.Button("Follow", GUILayout.ExpandWidth(false)))
                        {
                            ConfigSettings.instance.ignoreFileName.Remove(relative);
                        }
                    }
                    //忽略文件
                    else
                    {
                        if (GUILayout.Button("Ignore", GUILayout.ExpandWidth(false)))
                        {

                            ConfigSettings.instance.ignoreFileName.Add(relative);
                        }
                    }

                    GUILayout.EndHorizontal();
                },
                (p) =>
                {
                    //获取拓展名
                    string str = Path.GetExtension(p);
                    //过滤编辑中的文件
                    bool isEditing = Path.GetFileName(p).StartsWith("~");
                    //过滤xlsx或xls文件
                    bool isXlsx = (str == ".xlsx" || str == ".xls");
                    //过滤结果
                    bool result = isXlsx && !isEditing;

                    string relative = Path.GetRelativePath(EditorHelper.ProjectPath, p.Replace("~$", ""));
                    //是否锁定
                    locked = locked || (isEditing && !ConfigSettings.instance.ignoreFileName.Contains(relative));

                    return result;
                });

            GUILayout.EndScrollView();


        }


        private void OnInspectorUpdate()
        {
            //移除忽略过的无效路径
            for (int i = 0; i < ConfigSettings.instance.ignoreFileName.Count; i++)
            {
                if (!File.Exists(Path.Combine(EditorHelper.ProjectPath, ConfigSettings.instance.ignoreFileName[i])))
                {
                    Debug.Log($"Remove invalid excel path : {ConfigSettings.instance.ignoreFileName[i]}");

                    ConfigSettings.instance.ignoreFileName.RemoveAt(i);

                    i--;

                }
            }
        }
        string GetModelName(string x)
        {
            //获取Model名称
            var modelName = Path.GetFileNameWithoutExtension(x);
            modelName = modelName[0].ToString().ToUpper() + modelName.Substring(1, modelName.Length - 1);
            return modelName;
        }
        string GetGenPath(string x)
        {
            return Path.Combine(EditorHelper.ProjectPath, ConfigSettings.instance.generatePath, GetModelName(x) + "Model.cs");
        }
        private void GenerateAll()
        {
            if (locked) EditorUtility.DisplayDialog("Access permission conflict", "Please close excel", "I know");
            //EditorHelper.NotExistCreate(ConfigCache.instance.generatePath);

            //初始化筛选器
            FilterStrategy.InitFilters();
            SyntaxStrategy.InitTypes();

            string path = Path.Combine(EditorHelper.ProjectPath, ConfigSettings.instance.originPath);
            EditorHelper.NotExistCreate(path);
            EditorHelper.ForeachDFS(path, (x) =>
            {

                //生成路径
                string genPath = GetGenPath(x);
                string modelName = GetModelName(x);

                //生成文件和Excel表系统文件信息
                FileSystemInfo genFile = new FileInfo(genPath);
                FileSystemInfo xlsFile = new FileInfo(x);
                //文件无变动，跳过
                if (genFile.LastWriteTimeUtc == xlsFile.LastWriteTimeUtc)
                {
                    return;
                }
                //忽略文件，跳过
                //if (ConfigSettings.instance.ignoreFileName.Contains(x)) return;

                //读取Excel表
                ExcelStream stream = new ExcelStream(x);
                stream.Read();

                //获取筛选后的表
                ITable<string> table = FilterStrategy.GetTable(stream.Data);

                //转模型代码
                string code = TableToModel.GetCode(table, modelName, modelName);
                stream.Dispose();
                //创建生成文件
                EditorHelper.NotExistCreate(genPath);
                //写入生成文件
                StreamWriter writer = new StreamWriter(genPath, false, System.Text.Encoding.UTF8);
                writer.Write(code);
                writer.Close();

                //触摸文件
                genFile.LastWriteTimeUtc = xlsFile.LastWriteTimeUtc = DateTime.UtcNow;

            },
            (p) =>
            {
                string str = Path.GetExtension(p);
                //xlsx结尾
                bool result = (str == ".xlsx" || str == ".xls")
                //文件名不开头是~
                && (!Path.GetFileName(p).StartsWith("~"))
                //不是忽略文件里的
                && (!ConfigSettings.instance.ignoreFileName.Contains(Path.GetRelativePath(EditorHelper.ProjectPath, p)));


                return result;
            });
            AssetDatabase.Refresh();
        }
    }
}