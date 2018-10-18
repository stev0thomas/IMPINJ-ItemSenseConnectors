using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ItemSense;
using System.Configuration;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;
using System;

namespace PalletBuilder
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            txtItemSenseUri.Text = ConfigurationManager.AppSettings["ItemSenseUri"];
            txtItemSenseUsername.Text = ConfigurationManager.AppSettings["ItemSenseUsername"];
            txtItemSensePassword.Text = ConfigurationManager.AppSettings["ItemSensePassword"];
            txtFacility.Text = ConfigurationManager.AppSettings["ZoneTransitionToFacilityFilter"];
            txtRunNbr.Text = ConfigurationManager.AppSettings["LastPalletId"];
            txtDbCon.Text = ConfigurationManager.AppSettings["DbConnectionString"];
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("The application must close and be restarted for changes to be applied.", "Requires Application Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;

                settings["ItemSenseUri"].Value = txtItemSenseUri.Text;
                settings["ItemSenseUsername"].Value = txtItemSenseUsername.Text;
                settings["ItemSensePassword"].Value = txtItemSensePassword.Text;
                settings["ZoneTransitionToFacilityFilter"].Value = txtFacility.Text;
                settings["LastPalletId"].Value = txtRunNbr.Text;
                settings["DbConnectionString"].Value = txtDbCon.Text;

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
                Application.Exit();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
