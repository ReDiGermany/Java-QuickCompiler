using System;
using System.IO;
using System.Runtime.InteropServices;


// https://www.meziantou.net/detecting-console-closing-in-dotnet.htm
namespace J
{
    class ExitHandler
    {
        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms686016.aspx
        [DllImport("Kernel32")]
        protected static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms683242.aspx
        protected delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

        protected enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        protected static bool Handler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    // https://stackoverflow.com/questions/8807209/deleting-multiple-files-with-wildcard
                    foreach (string f in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.class"))
                    {
                        File.Delete(f);
                    }
                    Environment.Exit(0);
                    return false;

                default:
                    return false;
            }
        }
    }
}
