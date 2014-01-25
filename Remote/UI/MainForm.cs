using System.Windows.Forms;
using Remote.Data;
using Remote.Providers.FileZilla;
using Remote.Providers.PuTTY;
using Remote.Providers.RDP;
using Remote.Providers.WinSCP;
using Remote.Util;

namespace Remote.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var sm = SessionManager.Instance;
            sm.AddProvider(new PuTTYSessionProvider());
            sm.AddProvider(new WinSCPSessionProvider());
            sm.AddProvider(new FileZillaSessionProvider());
            sm.AddProvider(new RDPSessionProvider());
            sm.Populate();
            sessionTree.SessionActionSelected += GuardedActionDispatch;
            sessionTree.Populate(sm);
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
    }
}