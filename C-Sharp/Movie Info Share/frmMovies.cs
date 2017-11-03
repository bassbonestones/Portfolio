// See frmMainLogin for program specification
// also see about form in designer or run-time for more information

// this class is for the Movies form, which is the main interface of the program

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace StonesJ_AdvancedDB
{
    public partial class frmMovies : Form
    {
        //variables for movies form class
        string mode = "view";
        bool signingOff;
        public frmMovies()
        {
            // default constructor
            InitializeComponent();
        }

        private void frmMovies_Load(object sender, EventArgs e)
        {
            // set inital directory of openFileDialog
            ofdMovies.InitialDirectory = "c:\\";
            // load movies of current user to start.
            DBOps.getMovies(DBOps.currentUser);
            dgvMovie.DataSource = DBOps.dsMovies.Tables[DBOps.dtMovies];
            DBOps.formatGrid(dgvMovie);
            // set up bool for proper form exit
            signingOff = false;
            // center movie info group box
            gbxMovie.Location = new Point(this.Width / 2 - gbxMovie.Width / 2, gbxMovie.Location.Y);
            RefreshFormView();
            // also load movie images into ImageList from file
            if(File.Exists("..\\..\\Images\\images.dat"))
            {
                StreamReader sr = File.OpenText("..\\..\\Images\\images.dat");
                string line;
                string[] tokens;
                while((line = sr.ReadLine()) != null)
                {
                    tokens = line.Split(',');
                    int index = int.Parse(tokens[0]);
                    string filename = tokens[1];
                    DBOps.imageFilenames.Add(filename);
                    DBOps.imageIndexes.Add(index);
                }
            }            
        }

        private void mnuFileSignOff_Click(object sender, EventArgs e)
        {
            // signing off closes the current form and shows the login form
            signingOff = true;
            Application.OpenForms[0].Show();
            MessageBox.Show("Portfolio Sign-in: \nusername = 'user' \npassword = 'password'");
            this.Close();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // properly exit application (first write image info to file)
            DBOps.writeImages();
            Application.Exit();
        }

        private void frmMovies_FormClosed(object sender, FormClosedEventArgs e)
        {
            // makes sure that an exit by the X button in the upper right corner will also
            // properly end the program (keeps running otherwise) 
            if (!signingOff)
            {
                // first write image info to file
                DBOps.writeImages();
                Application.Exit();
            }
        }

        private void mnuFileClear_Click(object sender, EventArgs e)
        {
            // clears all movie data displayed and clears gridview selection
            foreach(Control c in gbxMovie.Controls)
            {
                if(c.Name.Contains("txt"))
                {
                    ((TextBox)c).Clear();
                }
            }
            dgvMovie.ClearSelection();
            txtMovie.Focus();
            pbxMovie.ImageLocation = "..\\..\\Images\\noimage.png";
            pbxMovie.Tag = "..\\..\\Images\\noimage.png";
        }

        private void dgvMovie_SelectionChanged(object sender, EventArgs e)
        {
            // changed to SelectionChange instead of mouse click, so that the data displayed always
            // reflects the selected row (whether mouse or keyboard change)
            if(mode == "add")
            {
                dgvMovie.ClearSelection();
                return;
            }
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

                string movie = txtMovie.Text;

                int index = int.Parse(dgvMovie.CurrentRow.Cells[0].Value.ToString());
                bool found = false;
                for(int i = 0; i < DBOps.imageIndexes.Count; i++)
                {
                    if(DBOps.imageIndexes[i] == index)
                    {
                        pbxMovie.ImageLocation = DBOps.imageFilenames[i];
                        found = true;
                    }
                }
                if(!found)
                {
                    pbxMovie.ImageLocation = "..\\..\\Images\\noimage.png";
                }
                // set picture based on selected row
                //switch (movie)
                //{
                //    case "Batman Begins":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\batmanbegins.png";
                //        break;
                //    case "The Dark Knight":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\thedarkknight.png";
                //        break;
                //    case "The Dark Knight Rises":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\thedarkknightrises.png";
                //        break;
                //    case "Hackers":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\hackers.png";
                //        break;
                //    case "The Lord of the Rings: Fellowship of the Ring":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\fellowshipofthering.png";
                //        break;
                //    case "The Lord of the Rings: The Two Towers":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\thetwotowers.png";
                //        break;
                //    case "The Lord of the Rings: The Return of the King":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\thereturnoftheking.png";
                //        break;
                //    case "The Hobbit: An Unexpected Journey":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\anunexpectedjourney.png";
                //        break;
                //    case "The Hobbit: The Desolation of Smaug":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\thedesolationofsmaug.png";
                //        break;
                //    case "The Hobbit: The Battle of the Five Armies":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\thebattleoffivearmies.png";
                //        break;
                //    case "300":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\300.png";
                //        break;
                //    case "The Dark Crystal":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\thedarkcrystal.png";
                //        break;
                //    case "Enemy Mine":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\enemymine.png";
                //        break;
                //    case "Ernest Scared Stupid":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\ernestscaredstupid.png";
                //        break;
                //    case "Jaws":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\jaws.png";
                //        break;
                //    case "Krull":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\krull.png";
                //        break;
                //    case "Life Is Beautiful":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\lifeisbeautiful.png";
                //        break;
                //    case "Muzzy":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\muzzy.png";
                //        break;
                //    case "Serenity":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\serenity.png";
                //        break;
                //    case "The Princess Bride":
                //        pbxMovie.ImageLocation = "..\\..\\Images\\theprincessbride.png";
                //        break;
                //    default:
                //        pbxMovie.ImageLocation = "..\\..\\Images\\noimage.png";
                //        break;
                //}
            }
        }

        private void mnuEditAdd_Click(object sender, EventArgs e)
        {
            // change mode to add and refresh view
            mode = "add";
            RefreshFormView();
        }
        private void RefreshFormView()
        {
            // used to change control enabling and visibility depending on mode
            switch(mode)
            {
                case "add":
                    // if add mode, enabled textboxes and buttons and clear text
                    {
                        foreach(Control c in gbxMovie.Controls)
                        {
                            if(c.Name.ToString().Contains("txt"))
                            {
                                ((TextBox)c).Enabled = true;
                            }
                        }
                        btnFunctions.Text = "Add";
                        btnFunctions.Visible = true;
                        btnUpdateImage.Visible = true;
                        mnuEditAdd.Enabled = false;
                        mnuEditUpdate.Enabled = true;
                        mnuEditDelete.Enabled = true;
                        mnuEditView.Enabled = true;
                        mnuFileClear.PerformClick();
                        break;
                    }
                case "update":
                    // if update mode, enabled controls
                    {
                        foreach(Control c in gbxMovie.Controls)
                        {
                            if (c.Name.ToString().Contains("txt"))
                            {
                                ((TextBox)c).Enabled = true;
                            }
                        }
                        btnFunctions.Text = "Update";
                        btnFunctions.Visible = true;
                        btnUpdateImage.Visible = true;
                        mnuEditAdd.Enabled = true;
                        mnuEditUpdate.Enabled = false;
                        mnuEditDelete.Enabled = true;
                        mnuEditView.Enabled = true;
                        break;
                    }
                case "delete":
                    // if delete mode, textboxes
                    {
                        foreach (Control c in gbxMovie.Controls)
                        {
                            if (c.Name.ToString().Contains("txt"))
                            {
                                ((TextBox)c).Enabled = false;
                            }
                        }
                        btnFunctions.Text = "Delete";
                        btnFunctions.Visible = true;
                        btnUpdateImage.Visible = false;
                        mnuEditAdd.Enabled = true;
                        mnuEditUpdate.Enabled = true;
                        mnuEditDelete.Enabled = false;
                        mnuEditView.Enabled = true;
                        break;
                    }
                default:
                    {
                        // if view mode, disable controls
                        foreach (Control c in gbxMovie.Controls)
                        {
                            if (c.Name.ToString().Contains("txt"))
                            {
                                ((TextBox)c).Enabled = false;
                            }
                        }
                        btnFunctions.Text = "Delete";
                        btnFunctions.Visible = false;
                        btnUpdateImage.Visible = false;
                        mnuEditAdd.Enabled = true;
                        mnuEditUpdate.Enabled = true;
                        mnuEditDelete.Enabled = true;
                        mnuEditView.Enabled = false;
                        break;
                    }
            }
        }

        private void btnFunctions_Click(object sender, EventArgs e)
        {
            switch(mode)
            {
                case "add":
                    {
                        // if add mode set properties of DBOps class and add new movie
                        DBOps.movieName = txtMovie.Text.Trim();
                        DBOps.genre = txtGenre.Text.Trim();
                        DBOps.director = txtDirector.Text.Trim();
                        DBOps.star1 = txtStar1.Text.Trim();
                        DBOps.star2 = txtStar2.Text.Trim();
                        DBOps.star3 = txtStar3.Text.Trim();
                        DBOps.description = txtDescription.Text.Trim();
                        string testRunTime = txtRunTime.Text;
                        foreach (char ch in testRunTime)
                            if (!Char.IsNumber(ch))
                            {
                                MessageBox.Show("Invalid entry for run time.  Please only use a number in minutes.");
                                txtRunTime.Focus();
                                txtRunTime.SelectAll();
                                return;
                            }
                        if (testRunTime.Length < 1)
                            DBOps.runTime = 0;
                        else
                            DBOps.runTime = Int32.Parse(txtRunTime.Text.Trim());
                        string testYear = txtYear.Text;
                        foreach (char ch in testYear)
                            if (!Char.IsNumber(ch))
                            {
                                // test for numeric entry
                                MessageBox.Show("Invalid entry for year.  Please only use a 4-digit year.");
                                txtYear.Focus();
                                txtYear.SelectAll();
                                return;
                            }
                        if (testYear.Length != 4 && testYear.Length != 0)
                        {
                            // test for numeric entry
                            MessageBox.Show("Invalid entry for year.  Please only use a 4-digit year.");
                            txtYear.Focus();
                            txtYear.SelectAll();
                            return;
                        }
                        if (testYear.Length < 1)
                            DBOps.year = 0;
                        else
                            DBOps.year = Int32.Parse(txtYear.Text.Trim());
                        DBOps.rating = txtRating.Text.Trim();
                        // add movie
                        DBOps.AddMovie();
                        // get new ID of movie after add
                        DBOps.img = pbxMovie.Tag.ToString();
                        DBOps.imageIndexes.Add(DBOps.movieID);
                        DBOps.imageFilenames.Add(DBOps.img);
                        // display success message
                        MessageBox.Show("The Movie, " + DBOps.movieName + ", was successfully added.");
                        // refresh form
                        dgvMovie.Refresh();
                        mnuFileClear.PerformClick();
                        break;
                    }
                case "update":
                    {
                        // check for row selection, if none, return.
                        if (dgvMovie.SelectedRows.Count < 1)
                            return;
                        // if add mode set properties of DBOps class and add new movie
                        DBOps.movieID = Int32.Parse(dgvMovie.CurrentRow.Cells[0].Value.ToString());
                        DBOps.movieName = txtMovie.Text.Trim();
                        DBOps.genre = txtGenre.Text.Trim();
                        DBOps.director = txtDirector.Text.Trim();
                        DBOps.star1 = txtStar1.Text.Trim();
                        DBOps.star2 = txtStar2.Text.Trim();
                        DBOps.star3 = txtStar3.Text.Trim();
                        DBOps.description = txtDescription.Text.Trim();
                        string testRunTime = txtRunTime.Text;
                        foreach (char ch in testRunTime)
                            if (!Char.IsNumber(ch))
                            {
                                MessageBox.Show("Invalid entry for run time.  Please only use a number in minutes.");
                                txtRunTime.Focus();
                                txtRunTime.SelectAll();
                                return;
                            }
                        if (testRunTime.Length < 1)
                            DBOps.runTime = 0;
                        else
                            DBOps.runTime = Int32.Parse(txtRunTime.Text.Trim());
                        string testYear = txtYear.Text;
                        foreach (char ch in testYear)
                            if (!Char.IsNumber(ch))
                            {
                                // test for numeric entry
                                MessageBox.Show("Invalid entry for year.  Please only use a 4-digit year.");
                                txtYear.Focus();
                                txtYear.SelectAll();
                                return;
                            }
                        if (testYear.Length != 4 && testYear.Length != 0)
                        {
                            // test for numeric entry
                            MessageBox.Show("Invalid entry for year.  Please only use a 4-digit year.");
                            txtYear.Focus();
                            txtYear.SelectAll();
                            return;
                        }
                        if (testYear.Length < 1)
                            DBOps.year = 0;
                        else
                            DBOps.year = Int32.Parse(txtYear.Text.Trim());
                        DBOps.rating = txtRating.Text.Trim();
                        // add movie
                        DBOps.UpdateMovie();                        
                        // display success message
                        MessageBox.Show("The Movie, " + DBOps.movieName + ", was successfully updated.");
                        // refresh form
                        dgvMovie.Refresh();
                        mnuFileClear.PerformClick();
                        break;
                    }
                case "delete":
                    {
                        // check for row selection, if none, return.
                        if (dgvMovie.SelectedRows.Count < 1)
                            return;
                        // get index of selected row and delete movie with that movieID
                        Int32 indexOfMovie = Int32.Parse(dgvMovie.CurrentRow.Cells[0].Value.ToString());
                        DBOps.RemoveMovie(indexOfMovie);
                        // refresh form
                        DBOps.getMovies(DBOps.currentUser);
                        dgvMovie.Refresh();
                        mnuFileClear.PerformClick();
                        break;
                    }
                default:
                    break;
            }
        }

        private void mnuEditUpdate_Click(object sender, EventArgs e)
        {
            // change mode and refresh form view
            mode = "update";
            RefreshFormView();
        }

        private void mnuEditDelete_Click(object sender, EventArgs e)
        {
            // change mode and refresh form view
            mode = "delete";
            RefreshFormView();
        }

        private void mnuEditView_Click(object sender, EventArgs e)
        {
            // change mode and refresh form view
            mode = "view";
            RefreshFormView();
        }

        private void mnuViewMyMovies_Click(object sender, EventArgs e)
        {
            // change dataset and refresh dataGridView
            // used to view only current users movies
            DBOps.getMovies(DBOps.currentUser);
            dgvMovie.Refresh();
            mnuViewMyMovies.Enabled = false;
            mnuViewAllMovies.Enabled = true;
            mnuEdit.Enabled = true;
            DBOps.formatGrid(dgvMovie);
            RefreshFormView();
            mnuFileClear.PerformClick();
        }

        private void mnuViewAllMovies_Click(object sender, EventArgs e)
        {
            // change dataset and refresh dataGridView
            // used to view all movies from all users
            DBOps.getMovies();
            dgvMovie.Refresh();
            mnuViewMyMovies.Enabled = true;
            mnuViewAllMovies.Enabled = false;
            mnuEdit.Enabled = false;
            // make sure to change back to view mode to disable editing data
            mnuEditView.PerformClick();
            btnUpdateImage.Visible = true;
            DBOps.formatGrid(dgvMovie);
            mnuFileClear.PerformClick();
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            // create about form and show modal
            frmAbout form = new frmAbout();
            form.ShowDialog();
        }

        private void btnUpdateImage_Click(object sender, EventArgs e)
        {
            // event for image update button

            // check to see if there are no rows selected, if not, return
            if (dgvMovie.SelectedRows.Count < 1 && mode != "add")
                return;
            // create an input form so that the image can be uploaded via different methods
            using (frmImageAlter form = new frmImageAlter())
            {
                // show new form
                form.ShowDialog();
                // if user selected to load from internet (YES)
                if (form.dlg == DialogResult.Yes)
                {
                    // set image of picturebox to property of input form
                    pbxMovie.ImageLocation = form.fname;
                    // set tag to image name also for refresh
                    pbxMovie.Tag = form.fname;
                    // display message about having to wait to user
                    MessageBox.Show("Your image may take a while to load from the internet. Please be patient. You may click on the picture box to refresh it. Even if you don't see it load, it may still have loaded. You may always edit it later.");
                }
                // if user selected to pick image from file upload (OK)
                else if (form.dlg == DialogResult.OK)
                {
                    // create filestream objecst for reading and writing (could have had one, but this is more clear)
                    FileStream fsRead;
                    FileStream fsWrite;
                    // show openfile dialog so user can pick an image
                    if (ofdMovies.ShowDialog() == DialogResult.OK)
                    {
                        string filename = ofdMovies.FileName;
                        fsRead = File.OpenRead(filename);
                        int extPoint = filename.LastIndexOf('.');
                        string extension = filename.Substring(extPoint + 1, (int)filename.Length - extPoint - 1);
                        int slashPoint = filename.LastIndexOf('\\');
                        filename = filename.Substring(slashPoint + 1, (int)filename.Length - slashPoint - 1);
                        filename = "..\\..\\Images\\" + filename;
                        byte[] img = new byte[fsRead.Length];
                        fsRead.Read(img, 0, (Int32)(fsRead.Length));
                        fsRead.Close();
                        fsWrite = File.OpenWrite(filename);
                        fsWrite.Write(img, 0, (Int32)(img.Length));
                        fsWrite.Close();
                        pbxMovie.ImageLocation = filename;
                        pbxMovie.Tag = filename;
                    }
                    else
                        return;
                }
                // otherwiser user hit CANCEL so return
                else
                    return;
            }
            // if in view all movies mode or in update current user mode, go ahead and update the picture immediately
            // only in add do we want to update it later (upon add button press)
            if(mnuViewMyMovies.Enabled == true || mode == "update")
            {
                // update image index and filename association
                DBOps.img = pbxMovie.Tag.ToString();
                DBOps.movieID = int.Parse(dgvMovie.CurrentRow.Cells[0].Value.ToString());
                // get title of movie for message on successful update
                string title = dgvMovie.CurrentRow.Cells[1].Value.ToString();
                bool found = false;
                for (int i = 0; i < DBOps.imageIndexes.Count; i++)
                {
                    if (DBOps.imageIndexes[i] == DBOps.movieID)
                    {
                        DBOps.imageFilenames[i] = DBOps.img;
                        found = true;
                    }
                }
                // if there's no image associated with this already, create new items in lists
                if (!found)
                {
                    DBOps.imageFilenames.Add(DBOps.img);
                    DBOps.imageIndexes.Add(DBOps.movieID);
                }
                MessageBox.Show("Image was successfully updated for " + title + ".");
            }
        }

        private void pbxMovie_Click(object sender, EventArgs e)
        {
            // file upload from the web is very slow, so this allows users to refresh the picture by clicking on it
            pbxMovie.ImageLocation = pbxMovie.Tag.ToString();
            pbxMovie.Refresh();
        }
    }
}
