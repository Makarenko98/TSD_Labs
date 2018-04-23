using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.BLL.Utils
{
    public static class QueryUtils
    {
        public static string NewLine(this string str)
        {
            return str + Environment.NewLine;
        }

        public static string NewLineWithTab(this string str, int tabCount = 1)
        {
            return str + Environment.NewLine + new string('\t', 1);
        }
    }
}
