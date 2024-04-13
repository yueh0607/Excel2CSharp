using FFramework.MVVM.UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

namespace FFramework.MVVM.UnityEditor
{

    [ExcelTypeSyntax("Vector2", typeof(Vector2SyntaxAnalyser))]
    public class Vector2SyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "UnityEngine.Vector2";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {

            string pattern = @"\((-?\d+(\.\d+)?),(-?\d+(\.\d+)?)\)"; // 正则表达式模式
            Match match = Regex.Match(str.Replace(" ", string.Empty), pattern);
            if (match.Success) return $"new {GetDynamicTrueType(null)}({match.Groups[1] + "F"},{match.Groups[3] + "F"})";
            else throw new InvalidCastException($"\"{str}\"[key={key}] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
        }
    }


    [ExcelTypeSyntax("Vector2[]", typeof(Vector2ArraySyntaxAnalyser))]
    public class Vector2ArraySyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "UnityEngine.Vector2[]";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            string pattern = @"\((-?\d+(\.\d+)?),(-?\d+(\.\d+)?)\)"; // 正则表达式模式
            str = str.Remove(' ');
            MatchCollection matches = Regex.Matches(str, pattern);

            string merge = $"new UnityEngine.Vector2[{matches.Count}]{{#CONTENTS#}}";
            string contents = "";
            foreach (Match match in matches.Cast<Match>())
            {
                float r = float.Parse(match.Groups[1].Value);
                float g = float.Parse(match.Groups[3].Value);

                contents += $"new {typeof(UnityEngine.Vector2).FullName}({r}F,{g}F),";

            }
            contents = contents.TrimEnd(',');
            merge = merge.Replace("#CONTENTS#", contents);


            if (contents.Length == 0)
            {
                throw new InvalidCastException($"\"{str}\"[key={key}] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }

            return merge;
        }
    }
}