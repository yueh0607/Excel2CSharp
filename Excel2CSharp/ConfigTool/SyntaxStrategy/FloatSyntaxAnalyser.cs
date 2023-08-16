using System;

namespace AirEditor.Config
{
    public class FloatSyntaxAnalyser : ISyntaxAnalyser
    {
        public string TrueType => "System.ValueType.Float";
        public object StringToValue(string str, string type, int row, int column)
        {
            bool result = float.TryParse(str, out var value);
            if (!result)
            {
                throw new InvalidCastException($"\"{str}\" is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }
            return value;
        }
    }
}