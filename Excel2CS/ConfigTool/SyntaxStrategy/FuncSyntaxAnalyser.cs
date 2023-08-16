using AirEditor.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExcelTypeSyntax("func1", typeof(Func1SyntaxAnalyser))]
public class Func1SyntaxAnalyser : ISyntaxAnalyser
{
    string ISyntaxAnalyser.TrueType => "System.Func<float,float>";
    object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column)
    {
        return $"(x)=>{str}";
    }
}
[ExcelTypeSyntax("func2", typeof(Func2SyntaxAnalyser))]
public class Func2SyntaxAnalyser : ISyntaxAnalyser
{
    string ISyntaxAnalyser.TrueType => "System.Func<float,float,float>";

    object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column)
    {
        return $"(x,y)=>{str}";
    }
}
[ExcelTypeSyntax("func3", typeof(Func3SyntaxAnalyser))]
public class Func3SyntaxAnalyser : ISyntaxAnalyser
{
    string ISyntaxAnalyser.TrueType => "System.Func<float,float,float,float>";

    object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column)
    {
        return $"(x,y,z)=>{str}";
    }
}