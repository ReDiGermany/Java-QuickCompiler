using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace J
{
    class Program : ExitHandler
    {
        public Program(string[] args)
        {
            SetConsoleCtrlHandler(Handler, true);
            String file = getFileName(args);
            double timerLength = 1000.0;
            if (args[0] == "-w")
            {
                Console.WriteLine("File Watcher active!");
                if (args.Length == 3)
                {
                    try
                    {
                        timerLength = double.Parse(args[1]) * 1000;
                        Console.WriteLine("new Length = " + args[1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Wrong format for -w length. using default.");
                    }
                }

                //https://stackoverflow.com/questions/1329900/net-event-every-minute-on-the-minute-is-a-timer-the-best-option
                Console.WriteLine("Starting filewatcher...");
                String cache = "";
                while (true)
                {
                    Thread.Sleep((int)timerLength);
                    String test = File.GetLastWriteTime(file + ".java").ToString();
                    if (test != cache)
                    {
                        cache = test;
                        Console.WriteLine("CHANGE!\n\n");
                        compile(file);
                        Console.WriteLine("\n");
                    }
                    // do your stuff here
                }
            }
        }
        static String getFileName(string[] args)
        {
            return args[args.Length - 1].Split('.')[0];
        }
        public static void compile(string file)
        {
            run("javac", file + ".java");
            run("java", file);
            File.Delete(file + ".class");
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: j {filename}");
            }
            else if (args.Length == 1)
            {
                compile(getFileName(args));
            }
            else
            {
                new Program(args);
            }
        }
        static void run(string name, string args)
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
