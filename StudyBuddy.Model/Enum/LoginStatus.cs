namespace StudyBuddy.Model.Enum
{
    public enum LoginStatus
    {
        Success = 0,
        EmailNotVerified = 1,
        IncorrectCredentials = 2,
        UserNotFound = 3,
        InvalidApiResponse = 4,
        NoToken = 5,
        UndocumentedError = 6,
        AccountDisabled = 7,
        InvalidToken = 8,
    }

    /*
        0 -> alles ok
        1 -> login erfolgreich aber email nicht verifiziert
        2 -> falsche anmeldedaten
        3 -> user nicht gefunden
        4 -> invalid API response
        5 -> No Token or User in jsonstring passed to login from json/token passed to loginfromjson is invalid
        6 -> undocumented error
        7 -> Account is disabled
        8 -> invalid passwordreset/verifyemail Token
    */
}
