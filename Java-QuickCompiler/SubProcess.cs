using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace J
{
    public class SubProcess
    {
        private readonly string binaryName;

        public SubProcess(string binaryName)
        {
            this.binaryName = binaryName;
        }

        public void Run(string programArguments)
        {
            var binaryPath = GetFullPath(this.binaryName);
            
            if (binaryPath == null)
            {
                throw new Exception($"The executable \"{this.binaryName}\" does not exists");
            }
            
            var process = new Process
            {
                StartInfo =
                {
                    FileName = binaryPath,
                    Arguments = programArguments,
                    WindowStyle = ProcessWindowStyle.Maximized
                }
            };

            process.Start();
            process.WaitForExit();
        }

        public bool BinaryExistsInPath()
        {
            return GetFullPath(binaryName) != null;
        }

        // https://stackoverflow.com/a/3856090
        private static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }
    }
}