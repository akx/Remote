using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Remote.Data;
using Remote.Util;

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
                GenerateTabPage(settings);
            }
            var logPage = new TabPage { Text = "Log" };
            logPage.Controls.Add(new RichTextBox
            {
                Dock = DockStyle.Fill,
                Text = Encoding.UTF8.GetString(Program.DefaultLogStream.ToArray()),
                ReadOnly = true
            });
            tabControl.TabPages.Add(logPage);
        }

        private void GenerateTabPage(SettingsObject settings)
        {
            var tabPage = new TabPage {Text = settings.Name};
            var propertyGrid = new PropertyGrid
            {
                Dock = DockStyle.Fill,
                SelectedObject = settings,
                ToolbarVisible = false,
            };
            propertyGrid.PropertyValueChanged += delegate(object o, PropertyValueChangedEventArgs args)
            {
                string name = args.ChangedItem.PropertyDescriptor.Name;
                bool valid = true;
                try
                {
                    if (settings.ValidateChange(name, args.OldValue, args.ChangedItem.Value)) return;
                    throw new Problem("Invalid value.");
                }
                catch (Exception e)
                {
                    args.ChangedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, args.OldValue);
                    MessageBox.Show(this, e.Message, "Unable to set value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            };
            tabPage.Controls.Add(propertyGrid);
            tabControl.TabPages.Add(tabPage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var settings in SettingsManager.Instance.GetSettingsObjects())
            {
                if (!settings.Validate())
                {
                    MessageBox.Show("{0}: did not validate", settings.Name);
                    return;
                }
            }
            DialogResult = DialogResult.OK;
            SettingsManager.Instance.SaveSettings();
            Close();
        }
    }
}
