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
            SessionManager.Instance.Populate();
            sessionTree.SessionActionSelected += GuardedActionDispatch;
            sessionTree.Populate(SessionManager.Instance);
            menuStrip.Visible = false;
            toolStrip.Visible = false;
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
        }
    }
}