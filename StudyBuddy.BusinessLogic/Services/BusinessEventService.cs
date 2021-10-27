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
                OnChallengeAccepted(args.Payload as Challenge);

            OnProcessOtherEvents();

        }

        private void OnChallengeAccepted(Challenge obj)
        {
            /*
             * 1) Passende Badges finden --> diejenigen, die auch mindestens ein passendes Tag tragen
             * 2) Für jedes dieser Badges:
             * 3) Prüfen, ob der Nutzer diesen Badge bereits hat, wenn nein:
             * 4) Das Verhältnis der möglichen Challenges (alle die entsprechende Tags tragen) zu den bereits abgeschlossenen Badges ermitteln --> x
             * 5) Wenn x >= Coverage vom Badge, dann 
             */
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
