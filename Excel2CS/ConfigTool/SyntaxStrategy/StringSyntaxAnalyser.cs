using System;

namespace AirEditor.Config
{
    [ExcelTypeSyntax("string", typeof(StringSyntaxAnalyser))]
    public class StringSyntaxAnalyser : ISyntaxAnalyser
    {
        public string TrueType => "System.String";
        public object StringToValue(string str, string type, int row, int column)
        {
            return "\""+str+ "\"";
        }
    }
}