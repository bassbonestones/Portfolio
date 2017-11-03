using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_MediaLab
{
    public partial class frmSetPrefPage : Form
    {
        public static string Url;
        public static DialogResult Dlg;

        public frmSetPrefPage(string title, string p_currenturl)
        {
            InitializeComponent();
            this.CenterToScreen();
            pnlTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Bottom;
            lblTitle.Text = title;
            lblCurrent.Text += p_currenturl;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtURL.Text.Trim() == "")
            {
                MessageBox.Show("You must enter a URL before clicking 'OK'.");
                return;
            }
            Url = txtURL.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtURL_DoubleClick(object sender, EventArgs e)
        {
            // if textbox double-clicked, select all text
            txtURL.SelectAll();
        }

        private void txtURL_Enter(object sender, EventArgs e)
        {
            // if textbox tabbed into, select all text
            txtURL.SelectAll();
        }
    }
}
