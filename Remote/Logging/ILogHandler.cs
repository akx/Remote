using System.Diagnostics;
using System.IO;
using System.Text;

namespace Remote.Logging {
	public interface ILogHandler {
		void Handle(LogRecord lr);
	}

	public abstract class BaseStringFormattingHandler : ILogHandler {
	    public BaseStringFormattingHandler()
	    {
	        FormatString = "{Time} - {Logger}:{Level} - {FormattedMessage}";
	    }

	    public string FormatString { get; set; }

	    public void Handle(LogRecord lr) {
			HandleFormattedMessage(lr, StringInject.Inject(FormatString, lr));
		}

		protected abstract void HandleFormattedMessage(LogRecord lr, string message);
	}

	public class PrintToDebugHandler : BaseStringFormattingHandler {
		protected override void HandleFormattedMessage(LogRecord lr, string message) {
			Debug.Print(message);
		}
	}

	public class PrintToStreamHandler : BaseStringFormattingHandler {
	    private readonly StreamWriter _writer;

		public PrintToStreamHandler(Stream stream)
		{
		    _writer = new StreamWriter(stream, Encoding.UTF8);
		}

	    protected override void HandleFormattedMessage(LogRecord lr, string message) {
			lock (_writer) {
				_writer.WriteLine(message);
                _writer.Flush();
			}
		}
	}
}