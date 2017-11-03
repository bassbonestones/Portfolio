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
    public partial class frmMovies : Form
    {
        // this form class is for showing all movie details from the movies database

        public frmMovies()
        {
            // default constructor
            InitializeComponent();
        }

        private void frmMovies_Load(object sender, EventArgs e)
        {
            // when form loads, set data source for DGV and format DGV
            dgvMovie.DataSource = DBOps.dsMovies.Tables[DBOps.dtMovies];
            try
            {
                DBOps.formatGrid(dgvMovie);
            }
            catch(Exception)
            { // catch out of bounds exeptions when database isn't connecting 
            }
        }

        private void dgvMovie_SelectionChanged(object sender, EventArgs e)
        {
            // changed to SelectionChange instead of mouse click, so that the data displayed always
            // reflects the selected row (whether mouse or keyboard change)
            
            // if there is a row selected
            if (dgvMovie.SelectedRows.Count > 0)
            {
                //Have to use minus one or write a new query with a where clause and pass in the pk.
                // set the text of the form controls based on the selected row
                txtMovie.Text = dgvMovie.CurrentRow.Cells[1].Value.ToString();
                txtGenre.Text = dgvMovie.CurrentRow.Cells[2].Value.ToString();
                txtDirector.Text = dgvMovie.CurrentRow.Cells[3].Value.ToString();
                txtStar1.Text = dgvMovie.CurrentRow.Cells[4].Value.ToString();
                txtStar2.Text = dgvMovie.CurrentRow.Cells[5].Value.ToString();
                txtStar3.Text = dgvMovie.CurrentRow.Cells[6].Value.ToString();
                txtDescription.Text = dgvMovie.CurrentRow.Cells[7].Value.ToString();
                txtRunTime.Text = dgvMovie.CurrentRow.Cells[8].Value.ToString();
                txtYear.Text = dgvMovie.CurrentRow.Cells[9].Value.ToString();
                txtRating.Text = dgvMovie.CurrentRow.Cells[10].Value.ToString();
                
                
            }
        }
        
    }
}
