// Jeremy Stones
// Advanced C#
// Brad Willingham

// Media Lab
// 7/28/2017

// Program Specification
// This program is a media enhanced WebBrowser.  I attempt to add much of the functionality of
// a normal browser using a database to store user preference information as well as favorites
// and history.  In addition to browser capability, this program allows users to play any media
// that can be associated with Windows Media Player as well as play games.  The program is designed
// for user customization. Users may change the default theme/color scheme, alter their favorites 
// (including a favorites bar), and manipulate their history data.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Stones_MediaLab
{
    public partial class frmBrowingTab : Form
    {
        // form nested inside of browsing window (includes bookmarks bar, navigation and 
        // browser control

        // create reference to parent object (variable)
        private frmMain parent;
        // flag to keep unnecessary reloading if nav buttons are clicked
        bool navigating = false;
        // flag for firstTime to keep unnecessary loading from happening
        bool firstTimeOpen = true;
        // flag for application to show stop button while page loading
        bool loading = false;
        public frmBrowingTab(frmMain p_parent)
        {
            // constructor that passes parent object for parent manipulation
            // from child form -- not recommended for standard forms, but works
            // well in MDI because form is aleady tightly coupled
            InitializeComponent();
            //this.CenterToScreen();
            //pnlBrowserTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Bottom;

            //pnlBrowserContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Bottom;
            // set parent reference
            parent = p_parent;
        }

        public void updateTheme()
        {
            // reference the current theme from the parent form and set
            // image and color scheme
            mnuPreviousBtn.Image = parent.currentTheme.BackImg;
            mnuNextBtn.Image = parent.currentTheme.ForwardImg;
            mnuRefreshStopBtn.Image = parent.currentTheme.RefreshImg;
            mnuHomeBtn.Image = parent.currentTheme.HomeImg;
            mnuEmailBtn.Image = parent.currentTheme.EmailImg;
            mnuSearchBtn.Image = parent.currentTheme.SearchImg;
            mnuFavoritesBtn.Image = parent.currentTheme.HeartImg;
            mnuMediaBtn.Image = parent.currentTheme.MediaImg;
            mnuSettingBtn.Image = parent.currentTheme.SettingsImg;
            mnuBookmarksBar.BackColor = parent.currentTheme.ColorBg1;
            mnuNavigator.BackColor = parent.currentTheme.ColorBg2;
            // toggle buttons that change theme so current theme button
            // is unavailable

            // for default -- "Ampere"
            if (parent.currentTheme == Theme.AmpereTheme)
                mnuSettingsPrefsThemeAmpere.Enabled = false;
            else
                mnuSettingsPrefsThemeAmpere.Enabled = true;
            // for secondary -- "Ignition"
            if (parent.currentTheme == Theme.IgnitionTheme)
                mnuSettingsPrefsThemeIgnition.Enabled = false;
            else
                mnuSettingsPrefsThemeIgnition.Enabled = true;
        }
        
        public void ResizeURLBox(int width)
        {
            // method called scale the url box with the form as it grows and shrinks
                txtURL.Width = width-72;
        }

        private void txtURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            // method that tests for enter key press in txtURL
            string url = txtURL.Text;
            if (e.KeyChar == (char)Keys.Enter)
            {
                // call navigate event to go to page and do some stuff - see below
                Navigate(url);
                // add one to index of web browser page
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] += 1;
                // add one to count of web browser pages
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 1] += 1;
                // end event (silences noise)
                RefreshNavButtons();
                e.Handled = true;
            }
        }
        private void RefreshNavButtons()
        {
            // tests for index of navigated pages against the total pages navigated
            // and enables/disables the back/forward buttons appropriately
            if (parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] ==
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 1] - 1)
                mnuNextBtn.Enabled = false;
            else
                mnuNextBtn.Enabled = true;
            if (parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] < 1)
                mnuPreviousBtn.Enabled = false;
            else
                mnuPreviousBtn.Enabled = true;
            
        }
        public void Navigate(String address, bool newTab = false)
        {
            // sends user to new page, pulls the pages "favicon", and sets the tab information
            navigating = true;
            try
            {
                // try to get the "favicon" from the website to display in the tab
                // if the tab has just been opened, set the main page to google and get its icon
                if (newTab)
                    txtURL.Text = "www.google.com";
                // request custom google page that grabs the "favicon"
                string addr = txtURL.Text;
                Image ico = DBOps.GetFavicon(addr);
                // set the tab's associated icon
                parent.webIcos[parent.tabMain.SelectedIndex] = ico;
            }
            catch (Exception) { } // if no icon, it's okay...

            // create and destroy a new tab to refresh the tab drawing
            parent.MakeTab(true);
            // send parameters for resizing of the textbox for url, especially for program beginning
            // maximized
            ResizeURLBox(parent.Width);

            // test if there is an http or https prefix, if not add one for visual appeal
            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }

            // update the text with the new address (might be overridden by LocationURL)
            txtURL.Text = address;
            try
            {
                // send webbrowser to new address
                webMain.Navigate(address);
                // store the address in the tab's tag
                parent.tabMain.TabPages[parent.tabMain.SelectedIndex].Tag = address;
                // if success, change tab name
                string nub = getNub(address);
                parent.tabMain.TabPages[parent.tabMain.SelectedIndex].Text = "  " + nub + "         ";
            }
            catch (Exception)
            {
                // would return, but end of method anyways
            }
        }

        private void txtURL_Enter(object sender, EventArgs e)
        {
            // if tab into txtURL, select all text
            txtURL.SelectAll();
        }

        private void txtURL_DoubleClick(object sender, EventArgs e)
        {
            // if double click txtURL, select all text
            txtURL.SelectAll();
        }

        private void mnuPreviousBtn_Click(object sender, EventArgs e)
        {
            // set navigating flag to prevent unnecessary dual refresh
            navigating = true;
            try
            {   
                // return control to previous page
                webMain.GoBack();
                // subtract one to index of web browser page
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] -= 1;
            }
            catch (Exception)
            {
                // if GoBack fails, index is improperly set (happens occassionally
                // with webbrowser navigation.  this will reset the index properly
               parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] = 0;
               
            }
            // after navigation refresh the state of the navigation controls
            RefreshNavButtons();

        }

        private void mnuNextBtn_Click(object sender, EventArgs e)
        {
            // set navigating flag for unnecessary dual refresh
            navigating = true;
            try
            {   
                // send webbrowser control forward one page
                webMain.GoForward();
                // add one to index of web browser page
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] += 1;
            }
            catch (Exception)
            {
                // if failed, index is off, probably by one, recalibrate index
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 1] =
                    parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] + 1;
            }
            // after navigation, refresh the state of the navigation controls
            RefreshNavButtons();
        }

        private void mnuRefreshStopBtn_Click(object sender, EventArgs e)
        {
            // if refresh button is clicked
            if (loading)
            {
                // if currently loading a page, stop it, toggle loading flag, and change
                // image back to refresh image
                webMain.Stop();
                loading = false;
                mnuRefreshStopBtn.Image = parent.currentTheme.RefreshImg;
            }
            else
            {
                // if page is not loading, refresh it and reset the url textbox with the locationURL
                webMain.Refresh();
                txtURL.Text = webMain.LocationURL.ToString();
            }
        }

        private void mnuHomeBtn_Click(object sender, EventArgs e)
        {
            // navigate to user's preferred homepage,
            // else to default home
            webMain.Navigate(DBOps.CurrentUser.HomeURL);
        }

        private void mnuEmailBtn_Click(object sender, EventArgs e)
        {
            // navigate to user's preferred email page,
            // else to default email page
            webMain.Navigate(DBOps.CurrentUser.EmailURL);
        }

        private void mnuSearchBtn_Click(object sender, EventArgs e)
        {
            // navigate to user's preferred search page,
            // else to default search page
            webMain.Navigate(DBOps.CurrentUser.SearchURL);
        }

        private void frmBrowingTab_Load(object sender, EventArgs e)
        {
            // when this inner form begins, set the webbrowser control to silent
            // so we don't get those pesky script errors due to an outdated control
            webMain.Silent = true;
            SetUserPrefs();
        }

        private void webMain_Enter(object sender, EventArgs e)
        {
            // One of a few attempts to refresh the txtURL text with a proper URL
            // The redundancy of attempts has been pretty successful, whereas it fails
            // often otherwise
            txtURL.Text = webMain.LocationURL.ToString();
        }

        private void webMain_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
        {
            // when the page has fully loaded attempt to set the textbox for URL to the
            // current LocationURL of the WebBrowser control
            string address = webMain.LocationURL.ToString();
            txtURL.Text = address;
            if(!navigating && !firstTimeOpen)
            {
                // add one to index of web browser page
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 0] += 1;
                parent.pageInfoPerTab[parent.tabMain.SelectedIndex, 1] += 1;
            }
            // this flag check makes sure both arrows stay disabled upon initial form load
            if (!firstTimeOpen)
                RefreshNavButtons();
            else
                firstTimeOpen = false;
            // no longer navigating
            navigating = false;
            // since the page is fully loaded, reset to the refresh image instead of stop
            mnuRefreshStopBtn.Image = parent.currentTheme.RefreshImg;
            // this flag only works properly here, because it insures the page is done before
            // testing to see whether a stop or refresh image should be present in navigation
            loading = false;
            DBOps.AddHistory(address);
        }
        
        

        private void webMain_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
        {
            // before nagivating at all, set the button to stop -- it will reset to refresh after
            // page finishes loading
            mnuRefreshStopBtn.Image = parent.currentTheme.StopImg;
            // set loading boolean
            loading = true;
        }

        private void mnuSettingsPrefsThemeAmpere_Click(object sender, EventArgs e)
        {
            // menu option clicked to change theme to ampere, alters user preference
            if (parent.currentTheme != Theme.AmpereTheme)
            {
                parent.currentTheme = Theme.AmpereTheme;
                parent.updateTheme();
                DBOps.CurrentUser.Theme = "Ampere";
            }
        }

        private void mnuSettingsPrefsThemeIgnition_Click(object sender, EventArgs e)
        {
            // menu option clicked to change theme to ignition, alters user preference
            if (parent.currentTheme != Theme.IgnitionTheme)
            {
                parent.currentTheme = Theme.IgnitionTheme;
                parent.updateTheme();
                DBOps.CurrentUser.Theme = "Ignition";
            }
        }

        private void mnuSettingsTab_Click(object sender, EventArgs e)
        {
            // when user select from menu to make tab, call same function as tab button
            parent.MakeTab();
        }

        private void mnuHeartBookmark_Click(object sender, EventArgs e)
        {
            // when users chooses to add the current page, load the url, alter it for visual appeal
            // and clarity, load a dialog form, get the page icon, and store all info into favorites

            string address = webMain.LocationURL;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            string nub = "";
            bool onBar;
            DialogResult dlg;
            using (frmAddBookmark bm = new frmAddBookmark(address, getNub(address).Trim()))
            {
                // creates dialog input box to get bookmark preferences from user
                dlg = bm.ShowDialog();
                if (dlg == DialogResult.Cancel)
                    return;
                onBar = frmAddBookmark.onBar;
                address = frmAddBookmark.Url;
                nub = frmAddBookmark.Alias;
            }
            // add favorite to favorites list
            DBOps.AddFavorite(address, nub);
            //if user opted to keep the favorite on the bar as well, set it up
            if (onBar)
                parent.addToBar(address, nub);
        }

        private string getNub(string address)
        {
            // method to pull out the domain of an address, great as a default alias to the page
            // user may change the default alias in the frmAddBookmark dialog

            // tests for presence of periods and slashes, uses substrings to extract only
            // the domain name
            string nub;
            int dec = address.IndexOf('.');
            int firstslash = address.IndexOf('/');
            int dec2 = address.Substring(dec + 1).IndexOf('.');
            if (dec2 < 0)
            {
                nub = address.Substring(address.IndexOf('/') + 2, dec - (firstslash + 2));
            }
            else
                nub = address.Substring(dec + 1, dec2);
            if (nub.Length > 8)
                nub = nub.Substring(0, 8);
            return nub;
        }

        public void AddToBar(string url, string alias)
        {
            // method that actually adds the new Bookmark Bar Item
            bool add = true;
            foreach(ToolStripMenuItem test in mnuBookmarksBar.Items)
            {
                if (test.Tag.ToString() == url)
                    add = false;
            }
            if (add)
            {
                // create temporary toolstripitem
                ToolStripItem t = new ToolStripMenuItem();
                // set 
                t.Text = alias;
                t.Image = DBOps.GetFavicon(url);
                // fill tag with url to use upon removal
                t.Tag = url;
                // create mousedown event, lambda expression to pass arguments to event
                t.MouseDown += new MouseEventHandler((s, e) => BBar_Click(s, e, url));
                // add temp item to bar
                mnuBookmarksBar.Items.Add(t);
            }
        }
        private void DeleteBarButton(object sender, EventArgs e, ToolStripMenuItem t, int index)
        {
            // pull information from the passed object to pass to removal method
            string url = t.Tag.ToString();
            string alias = t.Text;
            // send information for removal from list
            DBOps.RemoveFavorite(url, alias);
            // remove toolstripmenuitem from favorites bar by reference
            parent.RemoveABarButton(index);
        }

        public void BarButtonBeGone(int index)
        {
            // extra method used to cascade deletion of buttons from all favorites bars in open tabs
            mnuBookmarksBar.Items.RemoveAt(index);
        }

        private void BBar_Click(object sender, MouseEventArgs e, string url)
        {
            // method to handle creating a context menu for an individual bookmark on the bar
            // used to delete the item
            switch(e.Button)
            {
                // mouse args holds info about which mouse button was pressed
                case MouseButtons.Right:
                    {
                        // if rightclick, create context menu for deletion of clicked item
                        // great code find! will definitely use again

                        // create reference to the sender object (we want this deleted)
                        ToolStripMenuItem itemClicked = (ToolStripMenuItem)sender;
                        int clickIndex = mnuBookmarksBar.Items.IndexOf(itemClicked);
                        // create context menu
                        ContextMenu cm = new ContextMenu();
                        // create menu item for the context menu
                        MenuItem delete = new MenuItem();
                        // set menu item text
                        delete.Text = "Delete This Bookmark";
                        // add menu item to context menu
                        cm.MenuItems.Add(delete);
                        // add a click even to the menu item, use lamda to pass parameters
                        delete.Click += new EventHandler((s, e2) => DeleteBarButton(s, e2, itemClicked, clickIndex));
                        // show the context menu, pass associated control, and point of appearance (relative to control itself)
                        cm.Show(mnuBookmarksBar, mnuBookmarksBar.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)));
                    }
                    break;
                case MouseButtons.Left:
                    // if left mouse, simply navigate to the url page passed to favorite when it was added to the bar
                    webMain.Navigate(url);
                    break;
                default:
                    return;
            }
            
        }

        private void mnuHeartShowbar_CheckedChanged(object sender, EventArgs e)
        {
            // use the checkstate of the menu item to toggle the bookmark bar visibility
            mnuBookmarksBar.Visible = mnuHeartShowbar.Checked;
        }

        private void mnuSettingsInfo_Click(object sender, EventArgs e)
        {
            // create a new about form and show to user
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        private void mnuSettingsLogin_Click(object sender, EventArgs e)
        {
            // method to log in, create a new login form and get info from it
            // to connect user
            DialogResult dlg;
            using (frmLogin loginForm = new frmLogin(parent))
            {
                // show form
                dlg = loginForm.ShowDialog(parent);
                if (dlg == DialogResult.Cancel)
                {
                    // return if not logged in
                    return;
                }
                else if (dlg == DialogResult.OK)
                {
                    // clear previous history and favorites to load new from user
                    DBOps.ClearHistory();
                    DBOps.Favorites.Clear();
                    // user connection to database information
                    DBOps.ConnectUser(frmLogin.Username, frmLogin.Password);
                    // set user prefs through parent here
                    parent.setUserPrefs();
                }
            }
            // used to make sure form stays maximized
            // needs bug fixing attention on my computer at least
            //parent.Hide();
            //parent.Show();
            // refresh all views with new users preferences as stored in database
            parent.setUserPrefs();
            // confirmation message
            MessageBox.Show("The username, " + DBOps.CurrentUser.Username + ", is now logged in.");

        }

        public void SetUserPrefs()
        {
            // sets all view based on user preferences of current user
            if(DBOps.CurrentUser.Username == "")
            {
                // if nobody is logged in change the menu items accordingly
                mnuSettingsLoginStatus.Text = "--Not Logged In--";
                mnuSettingsLogin.Enabled = true;
                mnuSettingsSwitchUser.Enabled = false;
                mnuSettingsLogout.Enabled = false;
                mnuSettingsPrefs.Enabled = false;
            }
            else
            {
                // if new user, change menu items to reflect this
                mnuSettingsLoginStatus.Text = "--" + DBOps.CurrentUser.Username + "--";
                mnuSettingsLogin.Enabled = false;
                mnuSettingsSwitchUser.Enabled = true;
                mnuSettingsLogout.Enabled = true;
                mnuSettingsPrefs.Enabled = true;
            }
            // set theme based on string from database in user preference of theme
            parent.currentTheme = (DBOps.CurrentUser.Theme == "Ampere" ? Theme.AmpereTheme : Theme.IgnitionTheme);
            // update theme view
            updateTheme();
            for (int i = mnuBookmarksBar.Items.Count - 1; i >= 1; i--)
            {
                // delete all bookmarks first
                mnuBookmarksBar.Items.RemoveAt(i);
            }
            foreach (UserFavorites f in DBOps.Favorites)
            {
                // user favorites list to repopulate bookmarks
                parent.addToBar(f.URL, f.Alias);
            }
        }

        private void mnuSettingsPrefsHomeCurrent_Click(object sender, EventArgs e)
        {
            // displays the homepage (set in user preferences)
            DBOps.CurrentUser.HomeURL = webMain.LocationURL;
            MessageBox.Show("Homepage URL Updated: " + DBOps.CurrentUser.HomeURL);
        }

        private void mnuSettingsPrefsEmailCurrent_Click(object sender, EventArgs e)
        {
            // displays the email page (set in user preferences)
            DBOps.CurrentUser.EmailURL = webMain.LocationURL;
            MessageBox.Show("Email URL Updated: " + DBOps.CurrentUser.EmailURL);
        }

        private void mnuSettingsPrefsSearchCurrent_Click(object sender, EventArgs e)
        {
            // dispays the search page (set in user preferences)
            DBOps.CurrentUser.SearchURL = webMain.LocationURL;
            MessageBox.Show("Search URL Updated: " + DBOps.CurrentUser.SearchURL);
        }

        private void mnuSettingsLogout_Click(object sender, EventArgs e)
        {
            // logs any user currently logged in out of the system and
            // resets view to defaults
            string user = DBOps.CurrentUser.Username;
            DBOps.UpdateUserUponDisconnect();
            DBOps.DisconnectUser();
            parent.setUserPrefs();
            // confirm log out message
            MessageBox.Show("The user, " + user + ", has been logged out.");
        }

        private void mnuSettingsSwitchUser_Click(object sender, EventArgs e)
        {
            // switch from one user to another directly

            // get old user name to display at end of method
            string user = DBOps.CurrentUser.Username;
            DialogResult dlg;
            using (frmLogin loginForm = new frmLogin(parent))
            {
                // open new login form
                dlg = loginForm.ShowDialog(parent);
                if (dlg == DialogResult.Cancel)
                {
                    // if not new user, then cancel method
                    return;
                }
                else if (dlg == DialogResult.OK)
                {
                    // if new user, disconnect old user and log new user in
                    // to get new user preferences from the database
                    DBOps.UpdateUserUponDisconnect();
                    DBOps.DisconnectUser();
                    DBOps.ConnectUser(frmLogin.Username, frmLogin.Password);
                    // set user prefs through parent here
                    parent.setUserPrefs();
                }
            }
            // methods to fix glitch
            parent.Hide();
            parent.Show();
            // display successful user swap
            MessageBox.Show("The user, " + user + ", was successfully logged off.\n" +
                DBOps.CurrentUser.Username + " is now logged in.");
        }

        private void mnuSettingsPrefsHomeManual_Click(object sender, EventArgs e)
        {
            // this methods sets the user homepage manually 
            
            // get current homepage to display in change form
            string addr = DBOps.CurrentUser.HomeURL;
            DialogResult dlg;
            using (frmSetPrefPage setForm = new frmSetPrefPage("Set New Homepage", addr))
            {
                // open new setForm form
                dlg = setForm.ShowDialog();
                if (dlg == DialogResult.Cancel)
                {
                    // return if no new url set
                    parent.Show();
                    return;
                }
                else if (dlg == DialogResult.OK)
                {
                    // set new url
                    DBOps.CurrentUser.HomeURL = frmSetPrefPage.Url;
                }
            }
            // to remove possible glitch
            parent.Hide();
            parent.Show();
            // display successful change of homepage url
            MessageBox.Show("New homepage successfully set for " + DBOps.CurrentUser.Username + 
                ": " + DBOps.CurrentUser.HomeURL);

        }

        private void mnuSettingsPrefsEmailManual_Click(object sender, EventArgs e)
        {
            // method to set the user email page manually

            // get old email url to display in confirm message at end of method
            string addr = DBOps.CurrentUser.EmailURL;
            DialogResult dlg;
            using (frmSetPrefPage setForm = new frmSetPrefPage("Set New Email Page", addr))
            {
                // display setForm
                dlg = setForm.ShowDialog();
                if (dlg == DialogResult.Cancel)
                {
                    // return if no new url
                    parent.Show();
                    return;
                }
                else if (dlg == DialogResult.OK)
                {
                    // set new email url
                    DBOps.CurrentUser.EmailURL = frmSetPrefPage.Url;
                }
            }
            // to prevent glitch
            parent.Hide();
            parent.Show();
            // display succesful url change of email page
            MessageBox.Show("New email page successfully set for " + DBOps.CurrentUser.Username +
                ": " + DBOps.CurrentUser.HomeURL);
        }

        private void mnuSettingsPrefsSearchManual_Click(object sender, EventArgs e)
        {
            // method to manually set the search page

            // get old address for display 
            string addr = DBOps.CurrentUser.SearchURL;
            DialogResult dlg;
            using (frmSetPrefPage setForm = new frmSetPrefPage("Set New Search Page", addr))
            {
                // show new setForm
                dlg = setForm.ShowDialog();
                if (dlg == DialogResult.Cancel)
                {
                    // return if no new url
                    parent.Show();
                    return;
                }
                else if (dlg == DialogResult.OK)
                {
                    // set new search page url
                    DBOps.CurrentUser.SearchURL = frmSetPrefPage.Url;
                }
            }
            // to prevent glitch
            parent.Hide();
            parent.Show();
            // display successful url change of search page
            MessageBox.Show("New search page successfully set for " + DBOps.CurrentUser.Username +
                ": " + DBOps.CurrentUser.HomeURL);
        }

        private void mnuSettingsHistory_Click(object sender, EventArgs e)
        {
            // display history form -- user can view history list and remove items
            frmHistory history = new frmHistory();
            history.Show();
        }

        private void mnuAudioVideo_Click(object sender, EventArgs e)
        {
            // open a file dialog to get a file to play in a new browser tab

            DialogResult dlg;
            // most used and supported windows media player types
            ofdMedia.Filter = "(Supported WMP Types)|*.asf;*.wma;*.wmv;*.wm;*.wmd;" +
                "*.avi;*.mpg;*.mpeg;*.mp3;*.m3u;*.mid;" +
                "*.midi;*.aiff;*.au;*.mov;*.m4a;*.mp4|All Files (*.*)|*.*";
            // display open dialog
            dlg = ofdMedia.ShowDialog();
            if(dlg == DialogResult.OK)
            {
                // open a new tab with media
                parent.MakeTab(ofdMedia.FileName);
            }
        }

        private void mnuSettingsHelp_Click(object sender, EventArgs e)
        {
            // create and display help form
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }

        private void mnuMediaGamesJoindots_Click(object sender, EventArgs e)
        {
            // Join the Dots game tab creation
            parent.MakeTab(0);
        }

        private void mnuMediaGamesDrivingforce4_Click(object sender, EventArgs e)
        {
            // game tab creation - driving force
            parent.MakeTab(1);
        }

        private void mnuMediaGamesBearadventure_Click(object sender, EventArgs e)
        {
            // game tab creation - bear
            parent.MakeTab(2);
        }

        private void mnuMediaGamesYetirampage_Click(object sender, EventArgs e)
        {
            // game tab creation - yeti
            parent.MakeTab(3);
        }

        private void mnuMediaGamesNinjashark_Click(object sender, EventArgs e)
        {
            // game tab creation - ninja shark
            parent.MakeTab(4);
        }

        private void mnuMediaGamesMidnightshark_Click(object sender, EventArgs e)
        {
            // game tab creation - midnight cinema
            parent.MakeTab(5);
        }
    }
}
