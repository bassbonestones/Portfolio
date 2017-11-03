using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab11
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            // overrides CreateParams for the the form to disable the X button
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            // center every control on load
            foreach(Control c in this.Controls)
            {
                c.Location = new Point(this.Width / 2 - c.Width / 2, c.Location.Y);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // close button closes form
            this.Close();
        }
    }
}
