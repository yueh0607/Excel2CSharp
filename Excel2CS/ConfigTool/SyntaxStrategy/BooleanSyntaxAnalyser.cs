using System;

namespace AirEditor.Config
{
    [ExcelTypeSyntax("bool",typeof(BooleanSyntaxAnalyser))]
    public class BooleanSyntaxAnalyser : ISyntaxAnalyser
    {
        public string TrueType => "System.Boolean";
        public object StringToValue(string str, string type, int row, int column)
        {
            bool result = bool.TryParse(str, out var value);
            if (!result)
            {
                throw new InvalidCastException($"\"{str}\" is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
            return value;
        }
    }
}