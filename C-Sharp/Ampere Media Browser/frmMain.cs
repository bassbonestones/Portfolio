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


using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_MediaLab
{
    // see above program specification
    public partial class frmMain : Form
    {
        // this main form is just a container for the tabcontrol
        // everything else is in the other forms of the project

        Rectangle newTabButtonLocation;
        public Image[] webIcos = new Image[10];
        List<frmBrowingTab> browsingForms = new List<frmBrowingTab>();
        public int[,] pageInfoPerTab = new int[10, 2];
        public Theme currentTheme = Theme.AmpereTheme;
        AxShockwaveFlashObjects.AxShockwaveFlash flash;
        AxWindowsMediaPlayer player;
        
        bool playerOpen;
        bool flashOpen;
        public frmMain()
        {
            // constructor for frmMain
            InitializeComponent();
            //this.CenterToScreen();
            playerOpen = false;
            flashOpen = false;
            //tabMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Bottom;
            DBOps.DisconnectUser();
            //this.Icon = DBOps.History[0].icon;
            foreach (Control c in this.Controls)
            {
                // this took me FOREVER, because this control actually doesn't have a
                // freaking name! The parent form mouseDown is swallowed up by this one
                if (c.ToString().ToLower().Contains("mdi"))
                    c.MouseDown += new MouseEventHandler(frmMain_MouseDown);
            }
        }
        public class TabControl2 : TabControl
        {
            protected override void WndProc(ref Message m)
            {
                // overrides the rectangle that the tabcontrol normally draws its
                // tabpages from this way they are a little bit bigger and there's
                // not a pesky, unwanted margin around whatever is docked inside it
                if (m.Msg == 0x1300 + 40)
                {
                    RECT rc = (RECT)m.GetLParam(typeof(RECT));
                    rc.Left -= 7;
                    rc.Right += 7;
                    rc.Top -= 2;
                    rc.Bottom += 7;
                    Marshal.StructureToPtr(rc, m.LParam, true);
                }
                base.WndProc(ref m);
            }
        }

        public void updateTheme()
        {
            for (int i = 0; i < tabMain.TabCount; i++)
                browsingForms[i].updateTheme();
            MakeTab(true);
        }

        public void setUserPrefs()
        {
            for (int i = 0; i < tabMain.TabCount; i++)
                browsingForms[i].SetUserPrefs();
            MakeTab(true);
        }

        public void addToBar(string p_url, string p_alias)
        {
            // add favorite to all tabs
            for (int i = 0; i < tabMain.TabCount; i++)
                browsingForms[i].AddToBar(p_url, p_alias);
        }

        public void RemoveABarButton(int p_index)
        {
            // remove all favorite from all tabs
            for (int i = 0; i < tabMain.TabCount; i++)
                browsingForms[i].BarButtonBeGone(p_index);
        }
        public struct RECT
        {
            // structure to hold rectangle information grabbed from
            // the LParam of a WndProc message
            public int Left, Top, Right, Bottom;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // set drawmode of tabs and initalize theme

            tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            for (int i = 0; i < 10; i++)
            {
                // no tabs have pages yet
                // start all web tabs as a default of index 0, count 1
                pageInfoPerTab[i, 0] = 0;
                pageInfoPerTab[i, 1] = 1;
            }
            MakeTab();
            Theme.setThemes();
            updateTheme();
        }

        private void MouseDown_tabMain(object sender, MouseEventArgs e)
        {
            // if user clicks x on tab, close tab

            for (int i = 0; i < this.tabMain.TabPages.Count; i++)
            {
                Rectangle r = tabMain.GetTabRect(i);
                //Getting the position of the "x" mark.
                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 12, 10);
                if (closeButton.Contains(e.Location))
                {
                    if(playerOpen)
                    {
                        if(this.tabMain.TabPages[i].Tag.ToString() == "player")
                        {
                            player.Dispose();
                            playerOpen = false;
                        }
                    }
                    if (flashOpen)
                    {
                        if (this.tabMain.TabPages[i].Tag.ToString() == "flash")
                        {
                            flash.Dispose();
                            flashOpen = false;
                        }
                    }
                    if (tabMain.TabPages.Count == 1)
                    {
                        Application.Exit();
                    }

                    this.tabMain.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        private void DrawItem_tabMain(object sender, DrawItemEventArgs e)
        {
            // update tabMain background color

            e.Graphics.DrawRectangle(new Pen(Brushes.Black, 5), new Rectangle(e.Bounds.Right - 13, e.Bounds.Top + 9, 5, 5));
            Font font = new Font("Arial", 8, FontStyle.Bold);
            e.Graphics.DrawString("x", font, new SolidBrush(currentTheme.ColorBg1), e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.tabMain.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
            if (tabMain.TabCount < 10)
            {
                Rectangle r = tabMain.GetTabRect(tabMain.TabPages.Count - 1);
                r.X = r.X + 140;
                r.Y = r.Y + 8;
                r.Width = 80;
                r.Height = 10;
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 10), new Rectangle(r.X, r.Y, r.Width, r.Height));
                e.Graphics.DrawString("Add New Tab", font, new SolidBrush(currentTheme.ColorBg1), r.X, r.Y - 2);
                newTabButtonLocation = r;
            }
            if (webIcos[e.Index] != null)
                e.Graphics.DrawImage(webIcos[e.Index], e.Bounds.Left + 4, e.Bounds.Top + 5);
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            // if user clicks add new tab, make a new browser tab
            if (newTabButtonLocation.Contains(e.Location))
            {
                // if the mouse is being clicked in the area of the new tab button
                // create a new tab and add it to the Tab Control
                MakeTab();
            }
        }
        public void MakeTab()
        {
            // makes a new normal browser tab
            string title = " New Tab        ";
            TabPage myTabPage = new TabPage(title);
            tabMain.TabPages.Add(myTabPage);
            tabMain.SelectedIndex = tabMain.TabCount - 1;

            // create browser form and show it
            frmBrowingTab newBrowse = new frmBrowingTab(this);
            newBrowse.MdiParent = this;
            newBrowse.updateTheme();
            browsingForms.Add(newBrowse);
            tabMain.TabPages[tabMain.TabCount - 1].Controls.Add(newBrowse);
            tabMain.TabPages[tabMain.TabCount - 1].Tag = "";
            newBrowse.Show();
            newBrowse.Dock = DockStyle.Fill;
            newBrowse.Navigate("http://www.google.com", true);
        }
        public void MakeTab(string file)
        {
            // makes a new tab for a media player

            if(playerOpen || flashOpen)
            {
                MessageBox.Show("Only one player tab or game tab allowed open at a time.");
                return;
            }
            // get alias
            int filelen = file.Length;
            int lastSlashIndex = file.LastIndexOf('\\');
            int smallPathLen = file.Length - (lastSlashIndex + 1);
            
            string title = file.Substring(lastSlashIndex + 1, smallPathLen);
            if (title.Length > 8)
                title = title.Substring(0, 8);
            title += "       ";
            TabPage myTabPage = new TabPage(title);
            tabMain.TabPages.Add(myTabPage);
            tabMain.SelectedIndex = tabMain.TabCount - 1;
            
            // create new media player and show it
            player = new AxWMPLib.AxWindowsMediaPlayer();

            tabMain.TabPages[tabMain.TabCount - 1].Controls.Add(player);
            tabMain.TabPages[tabMain.TabCount - 1].Tag = "player";
            //.Show();
            player.Dock = DockStyle.Fill;
            player.enableContextMenu = false;
            player.URL = file;
            playerOpen = true;
        }
        public void MakeTab(int gameNum)
        {
            // make a new tab for a game

            if (flashOpen || playerOpen)
            {
                MessageBox.Show("Only one player tab or game tab allowed open at a time.");
                return;
            }
            string file = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString();
            switch (gameNum)
            {
                // set game chosen file path
                case 0:
                    // Join the Dots
                    file += "\\Flash\\jointhedots.swf";
                    break;
                case 1:
                    // Driving Force 4
                    file += "\\Flash\\drivingforce4.swf";
                    break;
                case 2:
                    // Bear in super action adventure
                    file += "\\Flash\\bear.swf";
                    break;
                case 3:
                    // Yeti Rampage
                    file += "\\Flash\\yetirampage.swf";
                    break;
                case 4:
                    // Ninja Shark
                    file += "\\Flash\\ninjashark.swf";
                    break;
                case 5:
                    // Midnight Cinema
                    file += "\\Flash\\midnightcinema.swf";
                    break;
                default:
                    return;
            }
            // get file for alias
            int filelen = file.Length;
            int lastSlashIndex = file.LastIndexOf('\\');
            int smallPathLen = file.Length - (lastSlashIndex + 1);

            string title = file.Substring(lastSlashIndex + 1, smallPathLen);
            if (title.Length > 8)
                title = title.Substring(0, 8);
            title += "       ";
            TabPage myTabPage = new TabPage(title);
            tabMain.TabPages.Add(myTabPage);
            tabMain.SelectedIndex = tabMain.TabCount - 1;

            // create new flash and open it
            flash = new AxShockwaveFlashObjects.AxShockwaveFlash();
            tabMain.TabPages[tabMain.TabCount - 1].Controls.Add(flash);
            tabMain.TabPages[tabMain.TabCount - 1].Tag = "flash";
            //.Show();
            flash.Dock = DockStyle.Fill;
            flash.Movie = file;
            flash.Play();
            flashOpen = true;
        }

        public void fu(object sender, _WMPOCXEvents_OpenStateChangeEvent e)
        {
            // disposes media player
            ((AxWindowsMediaPlayer)(sender)).Dispose();
        }
        public void MakeTab(bool invisible)
        {
            // overload just to refresh icons
            TabPage myTabPage = new TabPage();
            myTabPage.Width = 0;
            tabMain.TabPages.Add(myTabPage);
            tabMain.TabPages.RemoveAt(tabMain.TabCount - 1);
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            // resizes url box of all tabs
            for (int i = 0; i < tabMain.TabCount; i++)
                browsingForms[i].ResizeURLBox(this.Width);
        }
    }
}
