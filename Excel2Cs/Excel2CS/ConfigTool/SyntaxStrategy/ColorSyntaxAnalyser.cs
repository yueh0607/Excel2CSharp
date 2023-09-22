using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AirEditor.Config
{
    [ExcelTypeSyntax("Color", typeof(ColorSyntaxAnalyser))]
    public class ColorSyntaxAnalyser : ISyntaxAnalyser
    {
        public string TrueType => "UnityEngine.Color";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key)
        {
            string pattern = @"\((-?\d+(\.\d+)?),(-?\d+(\.\d+)?),(-?\d+(\.\d+)?),(-?\d+(\.\d+)?)\)"; // 正则表达式模式
            Match match = Regex.Match(str.Replace(" ", string.Empty), pattern);

            if (match.Success)
            {
                float r = float.Parse(match.Groups[1].Value);
                float g = float.Parse(match.Groups[3].Value);
                float b = float.Parse(match.Groups[5].Value);
                float a = float.Parse(match.Groups[7].Value);

                if (r > 1) r /= 255f;
                if (g > 1) g /= 255f;
                if (b > 1) b /= 255f;
                if (a > 1) a /= 255f;

                return $"new {TrueType}({r}F, {g}F, {b}F, {a}F)";
            }
            else
            {
                throw new InvalidCastException($"\"{str}\"[key={key}] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
        }
    }

}