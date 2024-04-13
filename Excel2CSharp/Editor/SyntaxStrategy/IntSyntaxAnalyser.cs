using System;
using System.Collections.Generic;

namespace FFramework.MVVM.UnityEditor
{
    [ExcelTypeSyntax("int", typeof(IntSyntaxAnalyser))]
    public class IntSyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Int32";

        public object StringToValue(string str, string type, int row, int column, string key,Dictionary<string,string> parameters)
        {
            str = str.Replace(" ", "");
            bool result = int.TryParse(str, out var value);
            if (!result)
            {
                throw new InvalidCastException($"\"{str}\"[key={key}] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
            return value;
        }
    }

    [ExcelTypeSyntax("int[]", typeof(IntArraySyntaxAnalyser))]
    public class IntArraySyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Int32[]";
        public object StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            //str = str.Replace(" ", "");
            string[] parts = str.Split(SyntaxHelper.GetSeps(parameters));
            string merge = "";
            foreach(var part in parts)
            {
                bool result = int.TryParse(part, out var value);
                if (!result)
                {
                    throw new InvalidCastException($"\"{str}\"[key= {key} ] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
                }
                merge += $"{value},";
            }
            merge.TrimEnd(',');
            return $"new System.Int32[{parts.Length}]{{{merge}}}";
        }
    }
}