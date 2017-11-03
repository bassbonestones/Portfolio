//Jeremiah Stones
//Advanced C#
//Kathy Wilganowski

// Program Specification can be found on the about page during runtime.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab10
{
    public partial class frmAbout : Form
    {
        // This class only inherits Form.  When displayed it shows the program specification and meta-data.
        public frmAbout()
        {
            InitializeComponent();
            lblTitle.Location = new Point((this.Width - lblTitle.Width) / 2, lblTitle.Location.Y);
            pbxHR.Location = new Point((this.Width - pbxHR.Width) / 2, pbxHR.Location.Y);
            lblText.Location = new Point((this.Width - lblText.Width) / 2, lblText.Location.Y);
            btnClose.Location = new Point((this.Width - btnClose.Width) / 2, btnClose.Location.Y);
        }

        // create const variable for form param of disabled X button

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close button closes form.  No need to show, because it's always shown modally with another form under it.
            this.Close();
        }

    }


}
