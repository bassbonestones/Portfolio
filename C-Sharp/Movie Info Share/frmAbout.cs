// about form  - nothing more to see here :)

using System;
using System.Windows.Forms;

namespace StonesJ_AdvancedDB
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            // default constructor
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // close this modal form to return to Movies form
            this.Close();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            foreach(Control c in this.Controls)
            {
                // center everything horizontally, leave vertical of current position
                c.Location = new System.Drawing.Point(this.Width / 2 - c.Width / 2, c.Location.Y);
            }
        }
    }
}
