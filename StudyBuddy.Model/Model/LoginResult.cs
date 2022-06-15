
using System.Text.Json.Serialization;

namespace StudyBuddy.Model
{
    public class LoginResult 
    {
        public User User { get; set; }
        public int Status { get; set; }
        /*
            0 -> alles ok
            1 -> login erfolgreich aber email nicht verifiziert
            2 -> falsche anmeldedaten
            3 -> user nicht gefunden
            4 -> invalid API response
            5 -> No Token or User in jsonstring passed to login from json/token passed to loginfromjson is invalid
            6 -> undocumented error
            7 -> Account is disabled
        */
        public string Token { get; set; }
    }
}
