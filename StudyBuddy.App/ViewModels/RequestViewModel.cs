using System;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class RequestViewModel
    {
        public int ID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public int SenderID { get; set; }
        public int RecipientID { get; set; }
        public UserViewModel Sender { get; set; }
        public RequestType Type { get; set; }
        public int? ChallengeID { get; set; }
        public ChallengeViewModel Challenge { get; set; }

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
    }
}