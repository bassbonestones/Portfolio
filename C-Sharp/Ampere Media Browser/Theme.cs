using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_MediaLab
{
    public class Theme
    {
        public static Theme AmpereTheme = new Theme();
        public static Theme IgnitionTheme = new Theme();
        private Image _backImg;
        private Image _forwardImg;
        private Image _refreshImg;
        private Image _stopImg;
        private Image _homeImg;
        private Image _searchImg;
        private Image _settingsImg;
        private Image _mediaImg;
        private Image _emailImg;
        private Image _heartImg;
        private Color _colorBg1;
        private Color _colorBg2;

        #region Properties
        public Image BackImg
        {
            get
            {
                return _backImg;
            }

            set
            {
                _backImg = value;
            }
        }

        public Image ForwardImg
        {
            get
            {
                return _forwardImg;
            }

            set
            {
                _forwardImg = value;
            }
        }

        public Image RefreshImg
        {
            get
            {
                return _refreshImg;
            }

            set
            {
                _refreshImg = value;
            }
        }

        public Image StopImg
        {
            get
            {
                return _stopImg;
            }

            set
            {
                _stopImg = value;
            }
        }

        public Image HomeImg
        {
            get
            {
                return _homeImg;
            }

            set
            {
                _homeImg = value;
            }
        }

        public Image SearchImg
        {
            get
            {
                return _searchImg;
            }

            set
            {
                _searchImg = value;
            }
        }

        public Image SettingsImg
        {
            get
            {
                return _settingsImg;
            }

            set
            {
                _settingsImg = value;
            }
        }

        public Image MediaImg
        {
            get
            {
                return _mediaImg;
            }

            set
            {
                _mediaImg = value;
            }
        }

        public Image EmailImg
        {
            get
            {
                return _emailImg;
            }

            set
            {
                _emailImg = value;
            }
        }

        public Image HeartImg
        {
            get
            {
                return _heartImg;
            }

            set
            {
                _heartImg = value;
            }
        }

        public Color ColorBg1
        {
            get
            {
                return _colorBg1;
            }

            set
            {
                _colorBg1 = value;
            }
        }

        public Color ColorBg2
        {
            get
            {
                return _colorBg2;
            }

            set
            {
                _colorBg2 = value;
            }
        }
        
        #endregion Properties
        
        public static void setThemes()
        {
            Theme.AmpereTheme.BackImg = Image.FromFile("../../Images/leftampere.png");
            Theme.AmpereTheme.ForwardImg = Image.FromFile("../../Images/rightampere.png");
            Theme.AmpereTheme.RefreshImg = Image.FromFile("../../Images/refreshampere.png");
            Theme.AmpereTheme.StopImg = Image.FromFile("../../Images/stopampere.png");
            Theme.AmpereTheme.HomeImg = Image.FromFile("../../Images/homeampere.png");
            Theme.AmpereTheme.EmailImg = Image.FromFile("../../Images/envelopeampere.png");
            Theme.AmpereTheme.HeartImg = Image.FromFile("../../Images/heartampere.png");
            Theme.AmpereTheme.SearchImg = Image.FromFile("../../Images/searchampere.png");
            Theme.AmpereTheme.MediaImg = Image.FromFile("../../Images/mediaampere.png");
            Theme.AmpereTheme.SettingsImg = Image.FromFile("../../Images/cogampere.png");
            Theme.AmpereTheme.ColorBg1 = new Color();
            Theme.AmpereTheme.ColorBg1 = SystemColors.GradientActiveCaption;
            Theme.AmpereTheme.ColorBg2 = new Color();
            Theme.AmpereTheme.ColorBg2 = Color.SteelBlue;
            Theme.IgnitionTheme.BackImg = Image.FromFile("../../Images/leftignition.png");
            Theme.IgnitionTheme.ForwardImg = Image.FromFile("../../Images/rightignition.png");
            Theme.IgnitionTheme.RefreshImg = Image.FromFile("../../Images/refreshignition.png");
            Theme.IgnitionTheme.StopImg = Image.FromFile("../../Images/stopignition.png");
            Theme.IgnitionTheme.HomeImg = Image.FromFile("../../Images/homeignition.png");
            Theme.IgnitionTheme.EmailImg = Image.FromFile("../../Images/envelopeignition.png");
            Theme.IgnitionTheme.HeartImg = Image.FromFile("../../Images/heartignition.png");
            Theme.IgnitionTheme.SearchImg = Image.FromFile("../../Images/searchignition.png");
            Theme.IgnitionTheme.MediaImg = Image.FromFile("../../Images/mediaignition.png");
            Theme.IgnitionTheme.SettingsImg = Image.FromFile("../../Images/cogignition.png");
            Theme.IgnitionTheme.ColorBg1 = new Color();
            Theme.IgnitionTheme.ColorBg1 = Color.FromArgb(255, 224, 192);
            Theme.IgnitionTheme.ColorBg2 = new Color();
            Theme.IgnitionTheme.ColorBg2 = Color.FromArgb(255, 192, 128);
        }
    }

}
