using System;
using System.Collections.Generic;
using System.IO;

namespace Remote.Util
{
	static class Locator
	{
		private static bool _initialized = false;
		private static List<string> _paths;
		private static List<string> _exts;
		private static string _defaultProgramFiles;
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

		internal static string LocateExecutable(string name) {
			Initialize();
			var paths = new List<string>();
			paths.AddRange(_paths);
			paths.Add(Path.Combine(_programFiles, name));
			paths.Add(Path.Combine(_programFilesX86, name));

			foreach (var path in paths) {
				if (!Directory.Exists(path)) continue;
				foreach (var ext in _exts) {
					var fullPath = Path.Combine(path, name + ext);
					if (File.Exists(fullPath)) return fullPath;
				}
			}
			return null;
		}
	}
}
