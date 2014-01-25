using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Remote.Data;

namespace Remote.UI
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            Font = new Font("Segoe UI", 8);
            foreach (var settings in SettingsManager.Instance.GetSettingsObjects())
            {
                var tabPage = new TabPage { Text = settings.GetType().Name.Replace("SessionProvider", "") };
                var propertyGrid = new PropertyGrid
                {
                    Dock = DockStyle.Fill,
                    SelectedObject = settings,
                    ToolbarVisible = false,
                };
                tabPage.Controls.Add(propertyGrid);
                tabControl.TabPages.Add(tabPage);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            SettingsManager.Instance.SaveSettings();
            Close();
        }
    }
}
