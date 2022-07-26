using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace General.Utils
{
    public class Extends
    {
        public static string Join<T>(char splitString, IEnumerable<T> list, Func<T, string> retFunc)
        {
            StringBuilder stringBuilder = new StringBuilder();
            IEnumerator<T> enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                stringBuilder.Append(retFunc(enumerator.Current));
                stringBuilder.Append(splitString);
            }
            return stringBuilder.ToString().TrimEnd(splitString);
        }
    }
}
