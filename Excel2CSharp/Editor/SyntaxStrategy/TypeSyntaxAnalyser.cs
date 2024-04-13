using FFramework.MVVM.UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

namespace FFramework.MVVM.UnityEditor
{

    [ExcelTypeSyntax("Type", typeof(TypeSyntaxAnalyser))]
    public class TypeSyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Type";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            //如果要求类型必须存在
            if (parameters.TryGetValue("exist", out string result))
            {
                //注释
                string description = "/*If the target type cannot be found, it may be due to the current class generated not being able to access the specified type*/";

                //取得全部类型
                List<Type> selected = new List<Type>();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    foreach (var tp in types)
                    {
                        if (tp.Name == str) selected.Add(tp);
                    }
                }

                //返回同名类型及注释
                if (selected.Count == 1) return $"typeof({selected[0].FullName})" + description;
                if (selected.Count == 0) throw new InvalidDataException($"\"[key={{key}}，row={{row}},column={{column}}] no such type in any assembly:{str}");
                //多个同名类型
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var tp in selected) builder.Append(tp.Assembly + "  " + tp.FullName + ",");
                    string error = builder.ToString().TrimEnd(',');
                    throw new InvalidDataException($"[key={key}，row={row},column={column}] There are mutiple types with same name and ExcelReflectionAttribute:" + error);
                }
            }
            return $"System.Type.GetType(\"{str}\")";
        }
    }
    [ExcelTypeSyntax("Type[]", typeof(TypeArraySyntaxAnalyser))]
    public class TypeArraySyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Type[]";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            string[] parts = str.Split(SyntaxHelper.GetSeps(parameters));
            string merge = $"new System.Type[{parts.Length}]{{#CONTENT#}}";
            string content = "";


            //如果要求类型必须存在
            if (parameters.TryGetValue("exist", out string result) && result == "true")
            {
                //遍历Excel写好的类型
                foreach (var tp in parts)
                {
                    try
                    {
                        //查询类型
                        Type searchType = Type.GetType(type, true);
                        content += $"typeof({searchType.FullName}),";
                    }
                    catch
                    {
                        //失败代表类型查找不到
                        throw new InvalidDataException($"\"[key={{key}}，row={{row}},column={{column}}] no such type in any assembly:{tp}");
                    }
                }
            }
            else //不需要验证类型的真伪性，在运行时反射查找
            {
                //遍历Excel写好的类型
                foreach (var tp in parts)
                {
                    try
                    {
                        content += $"Type.GetType({tp}),";
                    }
                    catch
                    {
                        //失败代表类型查找不到
                        throw new InvalidDataException($"\"[key={{key}}，row={{row}},column={{column}}] no such type in any assembly:{tp}");
                    }
                }
            }
            content = content.TrimEnd(',');
            merge = merge.Replace("#CONTENT#", content);
            return merge;
        }
    }
}

