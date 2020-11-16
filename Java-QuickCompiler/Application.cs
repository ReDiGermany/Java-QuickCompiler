using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace J
{
    public class Application
    {
        private readonly SubProcess javaCompiler;
        private readonly SubProcess javaExecutable;

        public Application()
        {
            javaCompiler = new SubProcess("javac");
            javaExecutable = new SubProcess("java");
        }
        
        public void Run(IReadOnlyList<string> args)
        {
            if (javaCompiler.BinaryExistsInPath() == false)
            {
                Console.WriteLine("The 'javac' binary is not available!");
                Environment.Exit(1);
            }
            
            if (javaExecutable.BinaryExistsInPath() == false)
            {
                Console.WriteLine("The 'java' binary is not available!");
                Environment.Exit(1);
            }
            
            var fileName = "";
            var watchForChanges = false;
            
            switch (args.Count)
            {                
                case 0:
                    Console.WriteLine("usage: j {filename}");
                    return;
                case 1:
                    fileName = args[0];
                    break;
                case 2:
                    watchForChanges = args[0].Equals("-w");
                    fileName = args[1];
                    break;
            }
            
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} does not exists");
                Environment.Exit(1);
            }
            
            fileName = Path.GetFileNameWithoutExtension(fileName);

            if (watchForChanges == false)
            {
                Compile(fileName);
                return;
            }

            using (var filesystemWatcher = new FileSystemWatcher())
            {
                filesystemWatcher.Path = Directory.GetCurrentDirectory();
                filesystemWatcher.Filter = "*.java";
                filesystemWatcher.IncludeSubdirectories = true;
                filesystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

                filesystemWatcher.Changed += (sender, eventArgs) => this.Compile(fileName);
                
                Console.WriteLine("Start the filewatcher");
                
                filesystemWatcher.EnableRaisingEvents = true;
                
                while (true)
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void Compile(string fileName)
        {
            javaCompiler.Run($"{fileName}.java");
            File.Delete(fileName + ".class");
            javaExecutable.Run(fileName);
        }
    }
}