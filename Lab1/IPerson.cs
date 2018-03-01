using System;

namespace Lib
{
    public interface IPerson
    {
        string Name { get; set; }
        DateTime BirthDate { get; set; }
        void Greeting ();
    }
}