using System;
using System.Collections.Generic;

namespace FFramework.MVVM.UnityEditor
{
    [ExcelTypeSyntax("float", typeof(FloatSyntaxAnalyser))]
    public class FloatSyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Single";
        public object StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            str = str.Replace(" ", "");
            bool result = float.TryParse(str, out var value);
            if (!result)
            {
                throw new InvalidCastException($"\"{str}\"[key={key}] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
            return str+"F";
        }
    }

    [ExcelTypeSyntax("float[]", typeof(FloatArraySyntaxAnalyser))]
    public class FloatArraySyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Single[]";
        public object StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            //str = str.Replace(" ", "");
            string[] parts = str.Split(SyntaxHelper.GetSeps(parameters));
            string merge = "";
            foreach (var part in parts)
            {
                bool result = float.TryParse(part, out var value);
                if (!result)
                {
                    throw new InvalidCastException($"\"{str}\"[key={key}] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
                }
                merge += $"{value},";
            }
            merge.TrimEnd(',');
            return $"new System.Single[{parts.Length}]{{{merge}}}";
        }
    }
}