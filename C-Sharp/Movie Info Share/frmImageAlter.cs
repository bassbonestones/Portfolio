using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_AdvancedDB
{
    public partial class frmImageAlter : Form
    {
        byte[] imageBytes;
        public DialogResult dlg;
        public string fname;
        public frmImageAlter()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string url = txtURL.Text;
            int extPoint = url.LastIndexOf('.');
            string extension = url.Substring(extPoint + 1, (int)url.Length - extPoint - 1);
            extension = extension.ToLower();
            int slashPoint = url.LastIndexOf('/');
            string filename = url.Substring(slashPoint + 1, (int)url.Length - slashPoint - 1);

            if (extension != "png" && extension != "jpg" && extension != "gif" && extension != "jpeg" &&
                extension != "bmp" && extension != "ico")
            {
                MessageBox.Show("Invalid extension: " + extension);
                return;
            }

            using (var webClient = new WebClient())
            {
                 imageBytes = webClient.DownloadData(url);
            }// do something with imageBytes` }
            //var webClient = new WebClient();
            
            FileStream fsWrite;
            fsWrite = File.OpenWrite("..\\..\\Images\\" + filename);
            fsWrite.Write(imageBytes, 0, (Int32)(imageBytes.Length));
            fname = "..\\..\\Images\\" + filename;
            dlg = DialogResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dlg = DialogResult.Cancel;
            this.Close();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            dlg = DialogResult.OK;
            this.Close();
        }

        private void btnWeb_Click(object sender, EventArgs e)
        {
            btnFile.Visible = false;
            lblURL.Visible = true;
            txtURL.Visible = true;
            btnSubmit.Visible = true;
            txtURL.Focus();
        }

        private void txtURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnSubmit.PerformClick();
        }
    }
}
