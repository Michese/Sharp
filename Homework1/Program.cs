using System;

namespace Homework1
{
    using Logic;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;


    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Logic logic = new Logic();
            logic.start();
        }
    }
}
