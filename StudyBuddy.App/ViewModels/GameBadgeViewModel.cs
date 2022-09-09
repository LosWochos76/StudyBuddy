using StudyBuddy.Model;
using System;
using System.Linq;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class GameBadgeViewModel : GameBadge
    {
        public string FontFamily
        {
            get
            {
                string[] str = IconKey.Split(",");
                return str[0];
            }
        }

        public string Icon
        {
            get
            {
                string[] str = IconKey.Split(",", StringSplitOptions.RemoveEmptyEntries);
                char[] chars = str[1].Split(new[] { @"\u" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => (char)Convert.ToInt32(c, 16))
                    .ToArray();
                string iconkey = new(chars);
                return iconkey;
            }
        }

        public Color IconColor
        {
            get
            {
                string[] str = IconKey.Split(",");
                return Color.FromHex(str[2]);
            }
        }

        public string CoverageText
        {
            get
            {
                return (RequiredCoverage * 100).ToString() + "%";
            }
        }

        public string DateText
        {
            get
            {
                return Created.ToString("dd.MM.yyyy");
            }
        }
    }
}