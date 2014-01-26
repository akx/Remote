using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Remote.Logging;

// ReSharper disable InconsistentNaming
namespace Remote.Util
{
    public static class Locator
    {
        private static bool _initialized;
        private static readonly List<string> _paths = new List<string>();
        private static readonly List<string> _exts = new List<string> { ".EXE", ".COM" };
        private static string _programFiles;
        private static string _programFilesX86;
        private static readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
        private static readonly Logger _log = Logger.Get("Locator");

        private static void Initialize()
        {
            if (_initialized) return;
            var envPath = Environment.GetEnvironmentVariable("PATH");
            if (envPath != null) _paths.AddRange(envPath.Split(Path.PathSeparator));
            var envPathExt = Environment.GetEnvironmentVariable("PATHEXT");
            if (envPathExt != null) _exts.AddRange(envPathExt.Split(Path.PathSeparator));
            var defaultProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            _programFiles = Environment.GetEnvironmentVariable("ProgramW6432") ?? Environment.GetEnvironmentVariable("ProgramFiles") ?? defaultProgramFiles;
            _programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? defaultProgramFiles;
            _initialized = true;
        }

        public static string LocateExecutable(string name, IEnumerable<string> pathHints = null)
        {
            if (_cache.ContainsKey(name))
            {
                return _cache[name];
            }
            var exe = _cache[name] = _LocateExecutable(name, pathHints);
            
            if (exe != null)
            {
                _log.Success("Located: {0} = {1}", name, exe);
            }
            else
            {
                _log.Failure("Could not locate '{0}'", name);
            }
            return exe;
        }

        private static string _LocateExecutable(string name, IEnumerable<string> pathHints) {
            Initialize();
            var paths = new List<string>();
            paths.AddRange(_paths);
            paths.Add(Path.Combine(_programFiles, name));
            paths.Add(Path.Combine(_programFilesX86, name));
            if (pathHints != null)
            {
                foreach (var pathHint in pathHints)
                {
                    paths.Add(Path.Combine(_programFiles, pathHint));
                    paths.Add(Path.Combine(_programFilesX86, pathHint));
                }
            }

            foreach (var path in paths)
            {
                if (!Directory.Exists(path)) continue;
                foreach (var ext in _exts)
                {
                    var fullPath = Path.Combine(path, name + ext);
                    if (File.Exists(fullPath)) return Path.GetFullPath(fullPath);
                }
            }

            var key =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + name + ".exe");
            if (key != null)
            {
                using (key)
                {
                    return key.GetValue("").ToString();
                }
            }
            return null;
        }
    }
}