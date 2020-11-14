using System;
using System.Diagnostics;
using System.IO;

namespace J
{
    class Program : ExitHandler
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: j {filename}");
            }
            else
            {
                SetConsoleCtrlHandler(Handler, true);
                String file = args[0].Split('.')[0];
                run("javac", file+".java");
                run("java", file);
                File.Delete(file + ".class");
            }
        }
        static void run(string name,string args)
        {
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = name;
            process.StartInfo.Arguments = args;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.
        }
    }
}
