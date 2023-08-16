using AirEditor.Config;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;



namespace AirEditor
{
    public class ConfigCache : ScriptableSingleton<ConfigCache>
    {
        public string generatePath = "Assets/Project/Scripts/ConstModels";
        public string originPath = "ConfigDataTable";

        
    }

    public class ConfigWindow : EditorWindow
    {
        [MenuItem("ConfigTool", menuItem = "AirFramework/ConfigTool")]
        static void Open()
        {
            var window = GetWindow<ConfigWindow>();
            window.Show();
         
        }


        Vector2 pos = Vector2.zero;
        private void OnGUI()
        {

            ConfigCache.instance.generatePath = EditorGUILayout.TextField("GeneratePath", ConfigCache.instance.generatePath);
            ConfigCache.instance.originPath = EditorGUILayout.TextField("OriginPath", ConfigCache.instance.originPath);
            if (GUILayout.Button("Generate All"))
            {
                GenerateAll();
            }


            pos = GUILayout.BeginScrollView(pos);

            string path = Path.Combine(EditorHelper.ProjectPath, ConfigCache.instance.originPath);
            if(Directory.Exists(path)) 
            EditorHelper.ForeachDFS(path, (x) =>
            {
                GUILayout.TextArea(Path.GetFileName(x));
            });

            GUILayout.EndScrollView();


        }

        private void GenerateAll()
        {
            //EditorHelper.NotExistCreate(ConfigCache.instance.generatePath);

            string path = Path.Combine(EditorHelper.ProjectPath, ConfigCache.instance.originPath);
            EditorHelper.NotExistCreate(path);
            EditorHelper.ForeachDFS(path, (x) =>
            {
                ExcelStream stream = new ExcelStream(x);
                stream.Read();

                FilterStrategy.InitFilters();
                SyntaxStrategy.InitTypes();

                ITable<string> table = FilterStrategy.GetTable(stream.Data);
                var modelName = Path.GetFileNameWithoutExtension(x);


                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                modelName = textInfo.ToTitleCase(modelName);


                string code = TableToModel.GetCode(table, modelName, modelName);
                stream.Dispose();

                string genPath = Path.Combine(EditorHelper.ProjectPath, ConfigCache.instance.generatePath, modelName + "Model.cs");
                EditorHelper.NotExistCreate(genPath);
                StreamWriter writer = new StreamWriter(genPath, false, System.Text.Encoding.UTF8);
                writer.Write(code);
                writer.Close();
            },
            (p) => Path.GetExtension(p) == ".xlsx");
            AssetDatabase.Refresh();
        }
    }
}