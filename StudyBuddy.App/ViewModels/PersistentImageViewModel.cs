using System.IO;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class PersistentImageViewModel
    {
        public int? UserID { get; set; }
        public int? BadgeId { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public byte[] Content { get; set; }

        public static PersistentImageViewModel FromModel(PersistentImage obj)
        {
            var result = new PersistentImageViewModel();
            result.UserID = obj.UserID;
            result.BadgeId = obj.BadgeId;
            result.Name = obj.Name;
            result.Length = obj.Length;
            result.Content = obj.Content;
            return result;
        }

        public ImageSource ToImageSource()
        {
            var stream = new MemoryStream(Content);
            return ImageSource.FromStream(() => stream);
        }
    }
}
