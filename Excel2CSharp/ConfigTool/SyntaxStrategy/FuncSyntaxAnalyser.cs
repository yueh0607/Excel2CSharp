using AirEditor.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Func1SyntaxAnalyser : ISyntaxAnalyser
{
    string ISyntaxAnalyser.TrueType => "System.Func<float,float>";
    object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column)
    {
        return $"(x)=>{str}";
    }
}
public class Func2SyntaxAnalyser : ISyntaxAnalyser
{
    string ISyntaxAnalyser.TrueType => "System.Func<float,float,float>";

    object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column)
    {
        return $"(x,y)=>{str}";
    }
}
public class Func3SyntaxAnalyser : ISyntaxAnalyser
{
    string ISyntaxAnalyser.TrueType => "System.Func<float,float,float,float>";

    object ISyntaxAnalyser.StringToValue(string str, string type, int row, int column)
    {
        return $"(x,y,z)=>{str}";
    }
}