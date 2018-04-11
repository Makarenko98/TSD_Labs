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

            d.OnAdd += (o, e) => Console.WriteLine($"OnAdd event: Item added: " +
                $"{(e).Item.Key} - " +
                $"{(e).Item.Value}");
            d.OnRemove += (o, e) => Console.WriteLine($"OnRemove event: Item removed: " +
                $"{(e).Item.Key} - " +
                $"{(e).Item.Value}");

            d.OnClear += (o, e) => Console.WriteLine($"OnClear event: collection cleared");

            d.Add(1, "one");
            d.Add(2, "two");
            d.Add(3, "three");

            Console.WriteLine("Dictionary:");
            d.PrintDictionary();
            d.Remove(1);

            Console.WriteLine("d.ContainsKey(2): " + d.ContainsKey(2));

            Console.WriteLine("Keys:");
            foreach(var i in d.Keys)
                Console.WriteLine(i);

            Console.WriteLine("Values");
            foreach (var i in d.Values)
                Console.WriteLine(i);

            string val;
            Console.WriteLine("d.TryGetValue(5, out val)");
            if(!d.TryGetValue(5, out val))
                Console.WriteLine("Value by key 5 can not be obtained");
            else
                Console.WriteLine(val);

            if(!d.TryAdd(2, "2"))
                Console.WriteLine("Item could not be added");

            d.Clear();

            Console.ReadKey();
        }
    }
}
