using System;

namespace Lib
{
    public class Teacher : IPerson
    {

        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Subject { get; set; }
        public int Experience { get; set; }

        public Teacher(string name, string subject)
        {
            Name = name;
            Subject = subject;
        }
        public void Greeting ()
        {
            Console.WriteLine ("hello I'm " + Name + " - " + Subject + " teacher");
        }
    }
}