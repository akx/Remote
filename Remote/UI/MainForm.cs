using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Remote.Data;
using Remote.Util;

namespace Remote.UI
{
	public partial class MainForm : Form {
		private static readonly SessionAction DefaultLaunchAction = new SessionAction("launch");
		public MainForm() {
			InitializeComponent();
			var sm = SessionManager.Instance;
			sm.AddProvider(new PuTTYSessionProvider());
			sm.AddProvider(new WinSCPSessionProvider());
			sm.Populate();
			PopulateSessionTree();
		}

		private void SortNodes(TreeNodeCollection collection) {
			var nodes = new List<TreeNode>();
			foreach (var node in collection) nodes.Add((TreeNode)node);
			nodes.Sort((a, b) => String.Compare(a.Text, b.Text, StringComparison.InvariantCultureIgnoreCase));
			collection.Clear();
			collection.AddRange(nodes.ToArray());
			foreach (var node in nodes) if (node.Nodes.Count > 0) SortNodes(node.Nodes);
		}

		private void PopulateSessionTree() {
			SessionManager sm = SessionManager.Instance;
			sessionTree.Nodes.Clear();
			sessionTree.BeginUpdate();
			foreach (var session in sm.Sessions) {
				if (!session.ShowInList) continue;
				var contextMenu = new ContextMenuStrip {Tag = session};
				contextMenu.Items.Add(new ToolStripMenuItem { Text = String.Format("Launch ({0})", session.ProgramName), Tag = DefaultLaunchAction });
				contextMenu.Items.Add(new ToolStripSeparator());
				foreach (var provider in sm.Providers) {
					var items = provider.GetSessionActions(session);
					if(items != null) {
						foreach (var item in items) {
							contextMenu.Items.Add(new ToolStripMenuItem {Text = item.Text, Tag = item});
						}
					}
				}
				contextMenu.ItemClicked += SessionContextMenuItemClicked;
				var leaf = new TreeNode {Text = session.Name, Name = session.Name, Tag = session, ToolTipText = session.ToolTipText, ContextMenuStrip = contextMenu};
				var path = session.Name.Split(':', '/');
				var root = sessionTree.Nodes;
				for (var i = 0; i < path.Length - 1; i++) {
					var component = path[i];
					var test = root.Find(component, false);
					if (test.Length > 0) {
						root = test[0].Nodes;
					}
					else {
						var node = new TreeNode {Text = component, Name = component};
						root.Add(node);
						root = node.Nodes;
					}
				}
				root.Add(leaf);
			}
			SortNodes(sessionTree.Nodes);
			sessionTree.EndUpdate();
		}

		private void SessionContextMenuItemClicked(object sender, ToolStripItemClickedEventArgs ea) {
			var menu = sender as ContextMenuStrip;
			var item = ea.ClickedItem as ToolStripMenuItem;
			if(menu != null && item != null) {
				var session = menu.Tag as Session;
				var action = item.Tag as SessionAction;
				if(session != null && action != null) {
					action.Dispatch(session);
				}
			}
		}

		private void sessionTree_DoubleClick(object sender, EventArgs e) {
			var node = sessionTree.SelectedNode;
			if (node == null) return;
			var session = node.Tag as Session;
			if(session != null) {
				(new SessionAction("launch")).Dispatch(session);
			}
		}
	}
}
