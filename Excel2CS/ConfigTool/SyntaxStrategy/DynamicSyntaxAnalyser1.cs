using System;

namespace AirEditor.Config
{
    public class DynamicSyntaxAnalyser : ISyntaxAnalyser
    {
        public string TrueType => "System.Dynamic";
        public object StringToValue(string str, string type, int row, int column)
        {
            return "\""+str+ "\"";
        }
    }
}