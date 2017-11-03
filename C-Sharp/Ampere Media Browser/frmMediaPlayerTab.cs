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
    public partial class frmMediaPlayerTab : Form
    {

        public frmMediaPlayerTab(string file)
        {
            InitializeComponent();
            wmpPlayer.URL = file;
        }
        
        private void frmMediaPlayerTab_FormClosing(object sender, FormClosingEventArgs e)
        {
            wmpPlayer.Dispose();
        }
    }
}
