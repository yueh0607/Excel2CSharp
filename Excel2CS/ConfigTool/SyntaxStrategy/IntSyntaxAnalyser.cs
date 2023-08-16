using System;

namespace AirEditor.Config
{
    [ExcelTypeSyntax("int", typeof(IntSyntaxAnalyser))]
    public class IntSyntaxAnalyser : ISyntaxAnalyser
    {
        public string TrueType => "System.Int32";
        public object StringToValue(string str, string type, int row, int column)
        {
            bool result = int.TryParse(str, out var value);
            if (!result)
            {
                throw new InvalidCastException($"\"{str}\" is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
            return value;
        }
    }
}