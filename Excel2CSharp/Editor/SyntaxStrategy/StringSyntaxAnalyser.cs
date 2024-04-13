using System;
using System.Collections.Generic;
using System.IO;

namespace FFramework.MVVM.UnityEditor
{
    [ExcelTypeSyntax("string", typeof(StringSyntaxAnalyser))]
    public class StringSyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.String";
        public object StringToValue(string str, string type, int row, int column,string  key,Dictionary<string,string> parameters)
        {
            if (!str.Contains("\"")) return "\"" + str + "\"";
            return str;
        }
    }

    [ExcelTypeSyntax("string[]", typeof(StringArraySyntaxAnalyser))]
    public class StringArraySyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.String[]";
        public object StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            string[] parts = str.Split(SyntaxHelper.GetSeps(parameters));
            string merge = "";
            foreach (var part in parts)
            {
                if (!part.Contains("\"")) return "\"" + part+ "\"";
                merge +=part+",";
            }
            merge = merge.TrimEnd(',');
            return $"new System.String[{parts.Length}]{{{merge}}}";
        }
    }
}