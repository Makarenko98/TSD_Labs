using System;

namespace Lib
{
    public class Student : IPerson
    {
        public int Course { get; set; }
        public float AverageScore { get; set; }

        public int Stipend
        {
            get
            {
                if (AverageScore == 5)
                    return 1600;
                if (AverageScore >= 4)
                    return 1300;
                return 0;
            }
        }

        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public Student (string name, int course)
        {
            Name = name;
            Course = course;
        }

        public override string ToString ()
        {
            return Name + " " + Course + " course";
        }

        public string Greeting()
        {
            return "hello I'm "+ Name +" - Student";
        }
    }
}