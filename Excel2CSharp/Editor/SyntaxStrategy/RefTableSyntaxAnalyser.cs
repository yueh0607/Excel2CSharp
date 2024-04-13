using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFramework.MVVM.UnityEditor
{
    [ExcelTypeSyntax("RefTable", typeof(RefTableSyntaxAnalyser))]
    public class RefTableSyntaxAnalyser : ISyntaxAnalyser
    {
        public string GetDynamicTrueType(Dictionary<string, string> parameters)
        {
            //获取类型参数
            if (parameters.TryGetValue("type", out string name2))
            {
                return name2+"Item";
            }
            throw new InvalidOperationException("RefTableSyntaxAnalyser: type=??? is not found in parameters");
        }

        public object StringToValue(string str, string type, int row, int column, string key, Dictionary<string, string> parameters)
        {
            //获取语法类型  ，应该正好为type参数对应的值 ，例如XXItem,但是此处应该写XXModel.主键类型
            if (!parameters.TryGetValue("type", out string trueName))
            {
                throw new InvalidOperationException("RefTableSyntaxAnalyser: type=??? is not found in parameters");
            }
            //主键类型获取
            if (!parameters.TryGetValue("key", out string primiaryKey))
            {
                throw new InvalidOperationException("RefTableSyntaxAnalyser: key=??? is not found in parameters");
            }

            //合成此类型的类型符号
            string ps = primiaryKey;
            foreach (var p in parameters)
            {
                ps += $"#{p.Key}={p.Value}";
            }

            //根据类型符号取得初始化值
            string v = SyntaxStrategy.GetValue(str, ps, row, column, key).ToString();
            return $"FFramework.SingletonProperty<{trueName}Model>.Instance.DataMap[{v}]";
        }


    }


}