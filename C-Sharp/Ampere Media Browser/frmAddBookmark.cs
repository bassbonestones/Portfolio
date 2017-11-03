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
    public partial class frmAddBookmark : Form
    {
        public static string Url;
        public static string Alias;
        public static bool onBar;
        public static DialogResult Dlg;

        public frmAddBookmark(string url, string alias)
        {
            InitializeComponent();
            this.CenterToScreen();
            pnlTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Bottom;
            txtURL.Text = url;
            txtAlias.Text = alias;

            if (url.Length > 255)
            {
                MessageBox.Show("The URL has been truncated to fit the maximum 255 characters. " +
                    "Your address is most likely invalid. Consider revising before submission.");
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtURL.Text.Trim() == "")
            {
                MessageBox.Show("You must enter a URL before clicking 'OK'.");
                return;
            }
            if(txtAlias.Text.Trim() == "")
            {
                MessageBox.Show("You must enter an alias for the site before clicking 'OK'.");
                return;
            }
            Url = txtURL.Text;
            Alias = txtAlias.Text;
            onBar = true;

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

        private void txtAlias_DoubleClick(object sender, EventArgs e)
        {
            // if textbox double-clicked, select all text
            txtAlias.SelectAll();
        }

        private void txtAlias_Enter(object sender, EventArgs e)
        {
            // if textbox tabbed into, select all text
            txtAlias.SelectAll();
        }
    }
}
