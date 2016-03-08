using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model;

namespace ThreeOneSevenBee.ModelConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Template();
            Console.WriteLine(t.ToString());
        }
    }
}
