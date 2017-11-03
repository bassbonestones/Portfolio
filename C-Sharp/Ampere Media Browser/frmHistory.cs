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
    public partial class frmHistory : Form
    {
        public frmHistory()
        {
            InitializeComponent();
            this.CenterToScreen();
            pnlTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Bottom;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // close browser history
            this.Close();
        }

        private void clbHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmHistory_Load(object sender, EventArgs e)
        {
            foreach (UserHistory h in DBOps.History)
            {

                string s = h.Stamp + " - " + h.URL;
                clbHistory.Items.Add(s);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            DBOps.History.Clear();
            clbHistory.Items.Clear();
        }

        private void btnClearChecked_Click(object sender, EventArgs e)
        {

            int removeitem = -1;
            List<int> removeList = new List<int>();
            foreach (object j in clbHistory.CheckedItems)
            {
                int len = j.ToString().Length;
                int space = j.ToString().LastIndexOf(' ');
                int urllen = len - (space + 1);
                string url = j.ToString().Substring(space + 1, urllen).Trim();
                MessageBox.Show(url);
                int ctr = 0;
                removeitem = -1;
                foreach (UserHistory h in DBOps.History)
                {
                    if (h.URL == url)
                    {
                        removeitem = ctr;
                    }
                }
                if (removeitem > -1)
                    DBOps.History.RemoveAt(removeitem);
                ctr = 0;
                foreach (object obj in clbHistory.Items)
                {

                    if (obj.ToString() == j.ToString())
                    {
                        removeList.Add(ctr);
                    }

                    ctr++;
                }
            }
            removeList.Sort();
            for (int i = removeList.Count - 1; i >= 0; i--)
            {
                clbHistory.Items.RemoveAt(removeList[i]);
            }
        }
    }
}

