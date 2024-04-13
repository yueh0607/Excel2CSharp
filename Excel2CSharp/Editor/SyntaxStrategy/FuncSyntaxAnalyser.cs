using FFramework.MVVM.UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFramework.MVVM.UnityEditor
{
    [ExcelTypeSyntax("Func1", typeof(Func1SyntaxAnalyser))]
    public class Func1SyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Func<float,float>";
        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            return $"(x)=>{str}";
        }
    }
    [ExcelTypeSyntax("Func2", typeof(Func2SyntaxAnalyser))]
    public class Func2SyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Func<float,float,float>";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            return $"(x,y)=>{str}";
        }
    }
    [ExcelTypeSyntax("Func3", typeof(Func3SyntaxAnalyser))]
    public class Func3SyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Func<float,float,float,float>";

        object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            return $"(x,y,z)=>{str}";
        }
    }
}