using System;
using System.Collections.Generic;
namespace AirEditor.Config
{
    public class SyntaxStrategy
    {

        /// <summary>
        /// 策略表：在这俩进行类型转换器定义
        /// </summary>
        private static Dictionary<string, ISyntaxAnalyser> syntaxAnalyers = new Dictionary<string, ISyntaxAnalyser>()
    {
            { "int",new IntSyntaxAnalyser()},
            { "string",new StringSyntaxAnalyser()},
            { "float",new FloatSyntaxAnalyser()},
            { "bool",new BooleanSyntaxAnalyser()},
             { "func1",new Func1SyntaxAnalyser()},
             { "func2",new Func2SyntaxAnalyser()},
             { "func3",new Func3SyntaxAnalyser()},
    };


        /// <summary>
        /// 是否存在转换策略
        /// </summary>
        /// <param name="typeSign"></param>
        /// <returns></returns>
        public static bool HasConverter(string typeSign) => syntaxAnalyers.ContainsKey(typeSign);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="typeSign"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static object GetValue(string str, string typeSign, int row, int column)
        {
            if (!syntaxAnalyers.ContainsKey(typeSign))
            {
                throw new InvalidCastException($"Unsupported type :\"{typeSign}\" at guessing column \"{column + 1}\"");
            }
            return syntaxAnalyers[typeSign].StringToValue(str, typeSign, row, column);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="typeSign"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static string GetTrueType(string typeSign)
        {
            if (!syntaxAnalyers.ContainsKey(typeSign))
            {
                throw new InvalidCastException($"Unsupported type :{typeSign}");
            }
            return syntaxAnalyers[typeSign].TrueType;
        }
    }
}