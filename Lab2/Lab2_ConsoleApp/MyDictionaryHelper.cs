using Lab2_Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_ConsoleApp
{
    static class MyDictionaryHelper
    {
        public static void PrintDictionary<TKey, TValue>(this MyDictionary<TKey, TValue> d)
        {
            foreach (var i in d)
                i.PrintElement();
        }

        public static void PrintElement<TKey, TValue>(this KeyValuePair<TKey, TValue> item)
        {
            Console.WriteLine($"{item.Key} - {item.Value}");
        }
    }
}
