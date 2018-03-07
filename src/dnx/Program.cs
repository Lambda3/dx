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
                ShowHelp();
                return 1;
            }
            var (command, arguments) = (args[0], args.Skip(1).ToArray());
            var (found, binary) = FindBinaryInPath(command);
            if (!found)
                (found, binary) = Install(command);
            if (!found)
                return 1;
            return Run(binary, arguments);
        }

        private static void ShowHelp() => WriteLine(@"dnx - The .NET Runner tool

Usage:
dnx command arguments");

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

        private static (bool found, string binary) Install(string command)
        {
            var (dotnetFound, dotnet) = FindDotNet();
            if (!dotnetFound)
                return (false, null);
            var arguments = $"install tool -g {command}".Split(' ');
            var exitCode = Run(dotnet, arguments);
            if (exitCode != 0)
            {
                WriteErrorLine("Could not install.");
                return (false, null);
            }
            return FindBinaryInPath(command);
        }

        private static (bool found, string binary) FindDotNet() => FindBinaryInPath("dotnet");

        private static (bool found, string binary) FindBinaryInPath(string command)
        {
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            var paths = pathEnv.Split(Path.PathSeparator);
            foreach (var path in paths)
            {
                var correctPath = GetPath(path, command);
                if (File.Exists(correctPath))
                    return (true, correctPath);
            }
            return (false, null);
        }

        private static string GetPath(string path, string command) =>
            Path.Combine(path, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{command}.exe" : command);

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
