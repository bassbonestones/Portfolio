using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_LabMDI
{
    public partial class frmAbout : Form
    {
        // this form is a pop-up form that gives the user more information about
        // the application

        public frmAbout()
        {
            // default constructor
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // closes about form
            this.Close();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            // on load, center the contents horizontally
            foreach (Control c in this.Controls)
                c.Location = new Point(this.Width / 2 - c.Width / 2, c.Location.Y);
        }
    }
}
