using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM.UnityEditor
{
    public static class SyntaxHelper
    {
        public static char[] GetSeps(Dictionary<string,string> parameters,char defaultSep=',')
        {
            char[] seps;
            if (parameters.TryGetValue("sep", out string par))
            {
                seps = par.ToCharArray();
            }
            else
            {
                seps = new char[] {defaultSep };
            }
            return seps;
        }
    }
}