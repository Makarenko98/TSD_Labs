using System;
using Lib;

namespace App
{
    public class Program
    {
        static void Main (string[] args)
        {
            Student s = new Student ("Petro", 2);
            s.AverageScore = 4;
            System.Console.WriteLine (s.Name + " stipend = " + s.Stipend);
            IPerson p = new Student ("Ivan", 3);
            p.Greeting ();
            p = new Teacher ("Oleg", "Matan");
            p.Greeting ();
            Console.ReadKey ();
        }
    }
}