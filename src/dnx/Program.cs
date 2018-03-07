using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dnx
{
    class Program
    {
        private static ConsoleColor defaultConsoleColor;

        static int Main(string[] args)
        {
            Initialize();
            if (args.Length == 0)
            {
                WriteLine(@"dnx - The .NET Runner tool

Usage:
dnx command arguments");
                return 1;
            }
            var command = args[0];
            var arguments = args.Skip(1).ToArray();
            var (binary, found) = FindBinary(command);
            if (!found)
                (binary, found) = Install(command);
            if (!found)
                return 1;
            return Run(binary, arguments);
        }

        private static void Initialize() => defaultConsoleColor = Console.ForegroundColor;

        private static int Run(string binary, string[] arguments)
        {
            var startInfo = new ProcessStartInfo(binary, string.Join(' ', arguments))
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = new Process() { StartInfo = startInfo };
            process.OutputDataReceived += (sender, e) => WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) => WriteErrorLine(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return process.ExitCode;
        }

        private static (string, bool) Install(string command)
        {
            var (dotnet, dotnetFound) = FindDotNet();
            if (!dotnetFound)
                return (null, false);
            var arguments = $"install tool -g {command}";
            var startInfo = new ProcessStartInfo(dotnet, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = new Process() { StartInfo = startInfo };
            process.OutputDataReceived += (sender, e) => WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) => WriteErrorLine(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                WriteErrorLine("Could not install.");
                return (null, false);
            }
            return FindBinary(command);
        }

        private static (string, bool) FindDotNet() => FindBinary("dotnet");

        private static (string, bool) FindBinary(string command)
        {
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            var paths = pathEnv.Split(Path.PathSeparator);
            foreach (var path in paths)
            {
                var correctPath = GetPath(path, command);
                if (File.Exists(correctPath))
                    return (correctPath, true);
            }
            return (null, false);
        }

        private static string GetPath(string path, string command)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var commandForWindows = isWindows ? $"{command}.exe" : command; // todo linux
            return Path.Combine(path, commandForWindows);
        }

        private static readonly object sync = new object();

        private static void WriteLine(string message)
        {
            if (message == null)
                return;
            lock (sync)
                Console.WriteLine(message);
        }

        private static void WriteErrorLine(string message)
        {
            if (message == null)
                return;
            lock (sync)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Error.WriteLine(message);
                Console.ForegroundColor = defaultConsoleColor;
            }
        }
    }
}
