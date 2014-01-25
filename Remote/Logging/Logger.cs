using System;
using System.Collections.Generic;

namespace Remote.Logging {
    public enum LogLevel
    {
        Debug = 0,
        Info = 10,
        Success = 11,
        Warn = 20,
        Failure = 21,
        Error = 30,
        Emerg = 40
    };

    public class LogRecord
    {

        public LogLevel Level { get; private set; }
        public string Message { get; private set; }
        public object[] Params { get; private set; }
        public DateTime Time { get; private set; }
        public Logger Logger { get; private set; }

        public string FormattedMessage
        {
            get { return string.Format(Message, Params); }
        }

        private readonly Dictionary<string, object> _extra;

        public LogRecord(Logger originalLogger, LogLevel level, string message, object[] @params, Dictionary<string, object> extra)
        {
            Logger = originalLogger;
            Time = DateTime.UtcNow;
            Level = level;
            Message = message;
            Params = @params;
            _extra = extra;
        }

        public object GetExtra(string key)
        {
            object value = null;
            if (_extra != null) _extra.TryGetValue(key, out value);
            return value;
        }
    }

	public class Logger {
        private static readonly Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();
        public static readonly Logger Root = new Logger(null, "");

        public static Logger Get(string fqn)
        {
            if (String.IsNullOrEmpty(fqn)) return Root;
            if (Loggers.ContainsKey(fqn)) return Loggers[fqn];
            var dot = fqn.LastIndexOf('.');
            var parentFqn = (dot > 0 ? fqn.Substring(0, dot - 1) : "");
            var parent = Get(parentFqn);
            return (Loggers[fqn] = new Logger(parent, fqn));
        }

		public string FQN { get; private set; }
		public Logger Parent { get; private set; }
		private readonly List<ILogHandler> _handlers = new List<ILogHandler>();

		public Logger(Logger parent, string fqn) {
			Parent = parent;
			FQN = fqn;
		}

		public void Log(LogLevel level, string message, params object[] Params) {
			Handle(new LogRecord(this, level, message, Params, null));
		}

		public void Log(LogLevel level, string message, object[] Params, Dictionary<string, object> extra) {
			Handle(new LogRecord(this, level, message, Params, extra));
		}

		public void Debug(string message, params object[] Params) {
			Log(LogLevel.Debug, message, Params);
		}

		public void Info(string message, params object[] Params) {
			Log(LogLevel.Info, message, Params);
		}

		public void Warn(string message, params object[] Params) {
			Log(LogLevel.Warn, message, Params);
		}

		public void Error(string message, params object[] Params) {
			Log(LogLevel.Error, message, Params);
		}

        public void Success(string message, params object[] Params)
        {
            Log(LogLevel.Success, message, Params);
        }

        public void Failure(string message, params object[] Params)
        {
            Log(LogLevel.Failure, message, Params);
        }

		public void Emerg(string message, params object[] Params) {
			Log(LogLevel.Emerg, message, Params);
		}

		public void Exception(string message, Exception exc, params object[] Params) {
			message += "\n" + exc;
			Log(LogLevel.Error, message, Params, new Dictionary<string, object> {{"Exception", exc}});
		}


		private void Handle(LogRecord lr) {
			foreach (var h in _handlers) h.Handle(lr);
			if (Parent != null) Parent.Handle(lr);
		}

		public void AddHandler(ILogHandler logHandler) {
			if (!_handlers.Contains(logHandler)) _handlers.Add(logHandler);
		}

		public bool RemoveHandler(ILogHandler logHandler) {
			if (_handlers.Contains(logHandler)) {
				_handlers.Remove(logHandler);
				return true;
			}
			return false;
		}

		public override string ToString() {
			return FQN;
		}
	}
}