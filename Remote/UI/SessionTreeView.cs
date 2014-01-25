using System.Drawing;
using Remote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Remote.UI
{
    internal delegate void SessionActionSelectedDelegate(SessionAction action, Session session);

    internal class SessionTreeView : TreeView
    {
        private static readonly SessionAction DefaultLaunchAction = new SessionAction("launch");

        public event SessionActionSelectedDelegate SessionActionSelected;

        public SessionTreeView()
        {
            DoubleClick += HandleDoubleClick;
            ShowNodeToolTips = true;
        }

        private void SortNodes(TreeNodeCollection collection)
        {
            var nodes = new List<TreeNode>();
            foreach (var node in collection) nodes.Add((TreeNode) node);
            nodes.Sort((a, b) => String.Compare(a.Text, b.Text, StringComparison.InvariantCultureIgnoreCase));
            collection.Clear();
            collection.AddRange(nodes.ToArray());
            foreach (var node in nodes) if (node.Nodes.Count > 0) SortNodes(node.Nodes);
        }

        public void Populate(SessionManager sessionManager)
        {
            Nodes.Clear();
            BeginUpdate();
            var sessions = sessionManager.Sessions.ToList();
            sessions.Sort((s1, s2) => String.Compare(s1.Name, s2.Name, StringComparison.InvariantCultureIgnoreCase));
            ImageList = new ImageList();
            ImageList.Images.Add(IconGenerator.GenerateIcon(BackColor, "+"));
            ImageIndex = 0;
            SelectedImageIndex = 0;

            foreach (var session in sessions)
            {
                if (!session.ShowInList) continue;
                var contextMenu = new ContextMenuStrip {Tag = session};
                contextMenu.Items.Add(new ToolStripMenuItem
                {
                    Text = String.Format("Launch ({0})", session.ProgramName),
                    Tag = DefaultLaunchAction
                });
                contextMenu.Items.Add(new ToolStripSeparator());
                foreach (var provider in sessionManager.Providers)
                {
                    var items = provider.GetSessionActions(session);
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            contextMenu.Items.Add(new ToolStripMenuItem {Text = item.Text, Tag = item});
                        }
                    }
                }
                contextMenu.ItemClicked += SessionContextMenuItemClicked;
                var imageKey = session.ProgramName;
                if (!ImageList.Images.ContainsKey(imageKey))
                {
                    double hue = (imageKey.GetHashCode()%32)/32.0 * 360.0;
                    Color color = UiUtil.ColorFromHsv(hue, 0.3, 0.8);
                    Color color2 = UiUtil.ColorFromHsv(hue, 0.5, 0.8);
                    string iconText = session.ProgramAbbrev;
                    ImageList.Images.Add(imageKey, IconGenerator.GenerateIcon(color, iconText));
                    ImageList.Images.Add(imageKey + "_Select", IconGenerator.GenerateIcon(color2, iconText));
                }

                var leaf = new TreeNode
                {
                    Text = session.DisplayName,
                    Name = session.Name,
                    Tag = session,
                    ToolTipText = session.ToolTipText,
                    ContextMenuStrip = contextMenu,
                    ImageKey = imageKey,
                    SelectedImageKey = imageKey + "_Select",
                };
                var path = session.Name.Split(':', '/');
                var root = Nodes;
                for (var i = 0; i < path.Length - 1; i++)
                {
                    var component = path[i];
                    var test = root.Find(component, false);
                    if (test.Length > 0)
                    {
                        root = test[0].Nodes;
                    }
                    else
                    {
                        var node = new TreeNode {Text = component, Name = component};
                        root.Add(node);
                        root = node.Nodes;
                    }
                }
                root.Add(leaf);
            }
            SortNodes(Nodes);
            EndUpdate();
        }


        private void SessionContextMenuItemClicked(object sender, ToolStripItemClickedEventArgs ea)
        {
            var menu = sender as ContextMenuStrip;
            var item = ea.ClickedItem as ToolStripMenuItem;
            if (menu != null && item != null)
            {
                var session = menu.Tag as Session;
                var action = item.Tag as SessionAction;
                if (SessionActionSelected != null) SessionActionSelected(action, session);
            }
        }


        private void HandleDoubleClick(object sender, EventArgs e)
        {
            var node = SelectedNode;
            if (node == null) return;
            var session = node.Tag as Session;
            if (SessionActionSelected != null) SessionActionSelected(DefaultLaunchAction, session);
        }
    }
}