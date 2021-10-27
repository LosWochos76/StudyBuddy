using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class RequestViewModel : INotifyPropertyChanged
    {
        private UserViewModel sender;
        private ChallengeViewModel challenge;

        public int ID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public int SenderID { get; set; }
        public int RecipientID { get; set; }
        public RequestType Type { get; set; }
        public int? ChallengeID { get; set; }

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
                    return Sender.FullName + " möchte sich mit dir befreunden.";

                return Sender.FullName + " möchte, dass du die Herausforderung \""
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

        public static RequestViewModel FromModel(Request c)
        {
            return new RequestViewModel()
            {
                ID = c.ID,
                Created = c.Created,
                SenderID = c.SenderID,
                RecipientID = c.RecipientID,
                Type = c.Type,
                ChallengeID = c.ChallengeID
            };
        }

        public UserViewModel Sender
        {
            get { return sender; }
            set
            {
                sender = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Name");
            }
        }

        public ChallengeViewModel Challenge
        {
            get { return challenge; }
            set
            {
                challenge = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Name");
            }
        }
    }
}