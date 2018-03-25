using System;
using System.Collections.Generic;
using System.Linq;
using Lab2_Lib;

namespace Lab2_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDictionary<int, string> d = new MyDictionary<int, string>();
            Dictionary<int, string>
            var val = new KeyValuePair<int, string>(1, "one");
            d.Add(val.Key, val.Value);
            d.Add(3, "three");
            d.Add(2, "two");
            d.Remove(3);
            d.Add(4, "tree");
            d[1] = "asdas";
            Console.WriteLine(d.Contains(val));
            new KeyValuePair<string, string>(null, null);
            KeyValuePair<int, string>[] k = new KeyValuePair<int, string>[1]
                {
                    new KeyValuePair<int, string>(1, "1")
                };
            

            Console.WriteLine(k);

            Console.WriteLine();

            foreach (var i in d)
                Console.WriteLine($"{i.Key} {i.Value}");

            Console.ReadLine();
        }
    }
}
