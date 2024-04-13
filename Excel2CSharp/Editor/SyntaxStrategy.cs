using System;
using System.Collections.Generic;
using System.Reflection;

namespace FFramework.MVVM.UnityEditor
{
    public class SyntaxStrategy
    {

        /// <summary>
        /// 策略表：在这俩进行类型转换器定义
        /// </summary>
        private static Dictionary<string, ISyntaxAnalyser> syntaxAnalyers = new Dictionary<string, ISyntaxAnalyser>()
        {
            //{ "int",new IntSyntaxAnalyser()},
            //{ "string",new StringSyntaxAnalyser()},
            //{ "float",new FloatSyntaxAnalyser()},
            //{ "bool",new BooleanSyntaxAnalyser()},
            // { "func1",new Func1SyntaxAnalyser()},
            // { "func2",new Func2SyntaxAnalyser()},
            // { "func3",new Func3SyntaxAnalyser()},
        };

        public static void InitTypes()
        {
            syntaxAnalyers.Clear();
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in ass)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var m_filter = type.GetCustomAttribute<ExcelTypeSyntaxAttribute>();
                    if (m_filter != null)
                        syntaxAnalyers.Add(m_filter.Key, (ISyntaxAnalyser)Activator.CreateInstance(m_filter.SyntaxType));

                }

            }
        }


        /// <summary>
        /// 是否存在转换策略
        /// </summary>
        /// <param name="typeSign"></param>
        /// <returns></returns>
        public static bool HasConverter(string typeSign) => syntaxAnalyers.ContainsKey(typeSign);



        private static Dictionary<string, Dictionary<string, string>> par_cache = new Dictionary<string, Dictionary<string, string>>();
        
        /// <summary>
        /// 返回无参类型字符串
        /// </summary>
        /// <param name="typeSign">带参类型符号</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string CheckParamsCache(string typeSign)
        {
            string typeSignWithParameters = typeSign;
            string[] parts = typeSign.Split('#');

            //类型缓存中不存在，则重新获取类型参数信息
            if (!par_cache.ContainsKey(typeSignWithParameters))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                if (parts.Length > 1)
                {
                    //逐个匹配接下来的参数
                    for (int i = 1; i < parts.Length; i++)
                    {

                        //参数形式类似于   sep=|
                        string par = parts[i];
                        //参数长度不足，或者不包含等号
                        if (par.Length < 3 || !par.Contains('=')) throw new Exception($"parameter format error:{par}");

                        //取第一个等号
                        int id = par.IndexOf("=");
                        //裁断出参数名
                        string name = par.Substring(0, id);
                        //裁断参数内容
                        string content = par.Substring(id+1, par.Length - id - 1);
                        parameters.Add(name, content);
                    }
                }
                par_cache.Add(typeSignWithParameters, parameters);
            }
            return parts[0];
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="typeSign"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static object GetValue(string str, string typeSign, int row, int column,string key)
        {
            
            //至少有一个元素，即使是空串，移除其参数
            string typeSignWithoutParameters = CheckParamsCache(typeSign);

            //此时类型缓存一定包含其参数信息字典

            if (!syntaxAnalyers.ContainsKey(typeSignWithoutParameters))
            {
                throw new InvalidCastException($"Unsupported type :\"{typeSignWithoutParameters}\" at guessing column \"{column + 1}\"");
            }

            return syntaxAnalyers[typeSignWithoutParameters].StringToValue(str, typeSignWithoutParameters, row, column, key, par_cache[typeSign]);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="typeSign"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static string GetTrueType(string typeSign)
        {
            string typeSignWithoutParameters = CheckParamsCache(typeSign);
            if (!syntaxAnalyers.ContainsKey(typeSignWithoutParameters))
            {
                throw new InvalidCastException($"Unsupported type :{typeSignWithoutParameters}");
            }
            return syntaxAnalyers[typeSignWithoutParameters].GetDynamicTrueType(par_cache[typeSign]);
        }
    }
}