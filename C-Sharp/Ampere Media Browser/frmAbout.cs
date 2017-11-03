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
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                // center everything horizontally, leave vertical of current position
                c.Location = new System.Drawing.Point(this.Width / 2 - c.Width / 2, c.Location.Y);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // close this modal form to return to Movies form
            this.Close();
        }
    }
}
