using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Remote.Util
{
	static class Locator
	{
		private static bool _initialized;
		private static List<string> _paths;
		private static List<string> _exts;
		private static string _programFiles;
		private static string _programFilesX86;

		private static void Initialize() {
			if (_initialized) return;
			_paths = new List<string>();
			var envPath = Environment.GetEnvironmentVariable("PATH");
			if(envPath != null) _paths.AddRange(envPath.Split(Path.PathSeparator));
			_exts = new List<string> { ".EXE", ".COM" };
			var envPathExt = Environment.GetEnvironmentVariable("PATHEXT");
			if(envPathExt != null) _exts.AddRange(envPathExt.Split(Path.PathSeparator));
			var defaultProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			_programFiles = Environment.GetEnvironmentVariable("ProgramFiles") ?? defaultProgramFiles;
			_programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? defaultProgramFiles;
			_initialized = true;
		}

		internal static string LocateExecutable(string name, IEnumerable<string> pathHints=null) {
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

			foreach (var path in paths) {
				if (!Directory.Exists(path)) continue;
				foreach (var ext in _exts) {
					var fullPath = Path.Combine(path, name + ext);
					if (File.Exists(fullPath)) return fullPath;
				}
			}

		    var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + name + ".exe");
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
