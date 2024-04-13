using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace FFramework.MVVM.UnityEditor
{
    [ExcelTypeSyntax("bool", typeof(BooleanSyntaxAnalyser))]
    public class BooleanSyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Boolean";
        public object StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            str = str.Replace(" ","");
            if (str == "1"||str=="true"||str=="True"||str=="TRUE") return "true";
            if (str == "0"||str=="false"||str=="False"||str=="FALSE") return "false";
            throw new InvalidCastException($"\"{str}\"[key= {key} ] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
        }


    }

    [ExcelTypeSyntax("bool[]", typeof(BooleanArraySyntaxAnalyser))]
    public class BooleanArraySyntaxAnalyser : ISyntaxAnalyser
    {

        public string GetDynamicTrueType(Dictionary<string, string> parameters) => "System.Boolean[]";
        public object StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            //str = str.Replace(" ", "");
            char[] seps = SyntaxHelper.GetSeps(parameters);
            string[] parts = str.Split(seps);

            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (var part in parts)
            {
                if (str == "1" || str == "true" || str == "True" || str == "TRUE") builder.Append("true,");
                if (str == "0" || str == "false" || str == "False" || str == "FALSE") builder.Append("false,");
                throw new InvalidCastException($"\"{str}\"[key= {key} ] is not a valid data for \"{type}\" at guessing position[{row + 1},{column + 1}]");
            }

            return $"new System.Boolean[{parts.Length}]{{{builder.ToString().TrimEnd(',')}}}";
        }
    }
}