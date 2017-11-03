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
    public partial class frmHelp : Form
    {
        // this form is a pop-up form that gives the user more information about
        // how to use the application

        public frmHelp()
        {
            // default constructor
            InitializeComponent();
        }

        private void frmHelp_Load(object sender, EventArgs e)
        {
            // on load, center the contents horizontally
            foreach (Control c in this.Controls)
                c.Location = new Point(this.Width / 2 - c.Width / 2, c.Location.Y);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // closes help form
            this.Close();
        }
    }
}
