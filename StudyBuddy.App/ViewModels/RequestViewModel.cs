using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class RequestViewModel : Request, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Name
        {
            get
            {
                if (Type == RequestType.Friendship)
                    return Sender.Name + " möchte sich mit dir befreunden.";

                return Sender.Name + " möchte, dass du die Herausforderung \""
                    + Challenge.Name + "\" bestätigst.";
            }
        }

        public string TypeString
        {
            get
            {
                return Type == RequestType.Friendship ? "Freundschaftsanfrage" : "Bestätigungsanfrage";
            }
        }

        public string TypeSymbol
        {
            get
            {
                if (Type == RequestType.Friendship)
                    return FontAwesomeIcons.PeopleArrows;

                return FontAwesomeIcons.Tasks;
            }
        }

        public new UserViewModel Sender { get; set; }
        public new ChallengeViewModel Challenge { get; set; }
    }
}