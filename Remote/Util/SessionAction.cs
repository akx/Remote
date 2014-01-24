using System;
using Remote.Data;

namespace Remote.Util
{
	class SessionAction {
		private readonly string _action;
		private readonly object[] _data;
		private readonly string _text;

		public string Action {
			get { return _action; }
		}

		public object[] Data {
			get { return _data; }
		}

		public string Text {
			get {
				return _text;
			}
		}

		public virtual void Dispatch(Session session) {
			switch(_action) {
				case "launch":
					session.Launch();
					break;
				default:
					throw new NotImplementedException(String.Format("Action {0} not implemented.", _action));
			}
		}

		public SessionAction(string text, string action, object[] data) {
			_text = text;
			_action = action;
			_data = data;
		}

		public SessionAction(string text, string action) {
			_text = text;
			_action = action;
		}

		public SessionAction(string action) {
			_action = action;
			_text = action;
		}

	}
}
