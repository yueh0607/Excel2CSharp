using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FFramework.MVVM.UnityEditor
{
    [ExcelTypeSyntax("Color", typeof(ColorSyntaxAnalyser))]
    public class ColorSyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "UnityEngine.Color";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            string pattern = @"\((-?\d+(\.\d+)?),(-?\d+(\.\d+)?),(-?\d+(\.\d+)?),(-?\d+(\.\d+)?)\)"; // 正则表达式模式
            Match match = Regex.Match(str.Replace(" ", string.Empty), pattern);

            if (match.Success)
            {
                float r = float.Parse(match.Groups[1].Value);
                float g = float.Parse(match.Groups[3].Value);
                float b = float.Parse(match.Groups[5].Value);
                float a = float.Parse(match.Groups[7].Value);

                //如果指定了max值
                if (parameters.TryGetValue("max", out string value))
                {
                    if (!float.TryParse(value, out float m))
                        throw new InvalidCastException($"Error parameters max={value}");
                    r /= m;
                    g /= m;
                    b /= m;
                    a /= m;
                }
                else
                {
                    //没指定max值，如果有大于1的数字，按RGBA255计算，否则为RGBA1
                    if(r>1||g>1||b>1||a>1)
                    {
                        r /= 255f;
                        g /= 255f;
                        b /= 255f;
                        a /= 255f;
                    }
                }



                return $"new {typeof(UnityEngine.Color).FullName}({r}F, {g}F, {b}F, {a}F)";
            }
            else
            {
                throw new InvalidCastException($"\"{str}\"[key={key}] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
        }
    }
    [ExcelTypeSyntax("Color[]", typeof(ColorArraySyntaxAnalyser))]
    public class ColorArraySyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "UnityEngine.Color[]";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            string pattern = @"\((-?\d+(\.\d+)?),(-?\d+(\.\d+)?),(-?\d+(\.\d+)?),(-?\d+(\.\d+)?)\)"; // 正则表达式模式
            str = str.Remove(' ');
            MatchCollection matches = Regex.Matches(str, pattern);

            string merge = $"new UnityEngine.Color[{matches.Count}]{{#CONTENTS#}}";
            string contents = "";
            foreach (Match match in matches.Cast<Match>())
            {
                float r = float.Parse(match.Groups[1].Value);
                float g = float.Parse(match.Groups[3].Value);
                float b = float.Parse(match.Groups[5].Value);
                float a = float.Parse(match.Groups[7].Value);

                //如果指定了max值
                if (parameters.TryGetValue("max", out string value))
                {
                    if (!float.TryParse(value, out float m))
                        throw new InvalidCastException($"Error parameters max={value}");
                    r /= m;
                    g /= m;
                    b /= m;
                    a /= m;
                }
                else
                {
                    //没指定max值，如果有大于1的数字，按RGBA255计算，否则为RGBA1
                    if (r > 1 || g > 1 || b > 1 || a > 1)
                    {
                        r /= 255f;
                        g /= 255f;
                        b /= 255f;
                        a /= 255f;
                    }
                }

                contents += $"new {typeof(UnityEngine.Color).FullName}({r}F,{g}F,{b}F,{a}F),";

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