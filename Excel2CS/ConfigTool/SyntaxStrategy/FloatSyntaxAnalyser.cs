using System;

namespace AirEditor.Config
{
    [ExcelTypeSyntax("float", typeof(FloatSyntaxAnalyser))]
    public class FloatSyntaxAnalyser : ISyntaxAnalyser
    {
        public string TrueType => "System.Single";
        public object StringToValue(string str, string type, int row, int column)
        {
            bool result = float.TryParse(str, out var value);
            if (!result)
            {
                throw new InvalidCastException($"\"{str}\" is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
            return str+"F";
        }
    }
}