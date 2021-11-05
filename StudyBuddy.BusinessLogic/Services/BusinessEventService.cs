using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{

    /* ToDo: weiter implementieren
     * 1) Modell-Klasse "BusinessEvent" bauen, ID, Name, Typ und C# code
     * 2) Repository bauen, um das in der DB zu speichern
     * 3) Einen Controller bauen, um den Zugriff zu ermöglichen
     * 4) Im Backend den Admins eine Möglichkeit geben, solche Events anzulegen
     */
    class BusinessEventService : IBusinessEventService
    {
        private readonly IBackend backend;

        public BusinessEventService(IBackend backend)
        {
            this.backend = backend;
        }

        public void TriggerEvent(object sender, BusinessEventArgs args)
        {
            if (args.CurrentUser == null)
                args.CurrentUser = backend.CurrentUser;

            if (args.Type == BusinessEventType.ChallengeAccepted)
                OnChallengeAccepted(args.Payload as Challenge, args.CurrentUser);

            OnProcessOtherEvents();
        }

        private void OnChallengeAccepted(Challenge obj, User current_user)
        {
            var badges_of_user = backend.GameBadgeService.GetBadgesOfUser(current_user.ID);
            var badges = backend.Repository.GameBadges.GetBadgesForChallenge(obj.ID);

            foreach (var badge in badges)
            {
                if (!IsInList(badges_of_user, badge))
                {
                    var success_rate = backend.GameBadgeService.GetSuccessRate(badge.ID, current_user.ID);
                    if (success_rate.Success >= badge.RequiredCoverage)
                    {
                        backend.Repository.GameBadges.AddBadgeToUser(current_user.ID, badge.ID);

                        // ToDo: Neuigkeit erzeugen!
                    }
                }
            }
        }

        private bool IsInList(IEnumerable<GameBadge> list, GameBadge badge)
        {
            foreach (var b in list)
                if (b.ID.Equals(badge.ID))
                    return true;

            return false;
        }

        private void OnProcessOtherEvents()
        {
            /*
             * 1) BusinessEvents laden und checken, ob der passende Typ dabei ist.
             * 2) Passenden Events ausführen
             */
        }
    }
}
