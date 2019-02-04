using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenni
{
    class Program
    {
        static void Main(string[] args)
        {
            // convention: args[0], template @ ../args[0]-template

            if (args.Length < 1)
                throw new Exception("need target dir, then convention is: args[0], template @ ../args[0]-template");

            var dirTarget = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            var dirTemplate = Path.Combine(Directory.GetCurrentDirectory(), $"{args[0]}-template");

            Debug.WriteLine($"target folder {dirTarget}");
            Debug.WriteLine($"template folder {dirTemplate}");

            var filesTemplate = Directory.GetFiles(dirTemplate, "*", SearchOption.AllDirectories);
        }
    }
}
