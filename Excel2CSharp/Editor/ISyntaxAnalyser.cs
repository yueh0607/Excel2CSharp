using System;
using System.Collections.Generic;

namespace FFramework.MVVM.UnityEditor
{
    public interface ISyntaxAnalyser
    {

        public string GetDynamicTrueType(Dictionary<string, string> parameters);
        public object StringToValue(string str, string type, int row, int column,string key,Dictionary<string,string> parameters);
    }

}
