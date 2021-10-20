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

            /*
             * 1) BusinessEvents laden und checken, ob der passende Typ dabei ist.
             * 2) Passenden Events ausführen
             */
        }
    }
}
