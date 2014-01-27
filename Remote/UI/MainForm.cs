using System.Drawing;
using System.Windows.Forms;
using Remote.Data;
using Remote.Util;

namespace Remote.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Font = new Font("Segoe UI", 8);
			Icon = Icon.FromHandle(IconGenerator.GenerateIcon(Color.Firebrick, "R").GetHicon());
        	ShowIcon = true;
            SessionManager.Instance.Populate();
            sessionTree.SessionActionSelected += GuardedActionDispatch;
            RefreshData();
            menuStrip.Visible = false;
            toolStrip.Visible = false;
        	statusStrip.Visible = false;
        }

        private void RefreshData()
        {
            sessionTree.Populate(SessionManager.Instance);
        }

        private void GuardedActionDispatch(SessionAction action, Session session)
        {
            try
            {
                if (session != null && action != null)
                {
                    action.Dispatch(session);
                }
            }
            catch (Problem prob)
            {
                MessageBox.Show(this, prob.Message, "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            (new SettingsForm()).ShowDialog(this);
            RefreshData();
        }
    }
}