using System;
using System.IO;

namespace J
{
    static class Program
    {
        static void Main(string[] args)
        {            
            // Register the signal handler
            Console.CancelKeyPress += delegate
            {
                foreach (var f in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.class"))
                {
                    File.Delete(f);
                }
            };
            
            new Application().Run(args);
        }
    }
}
