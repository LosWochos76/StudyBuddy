# Dies ist der Quellcode des Gamification-Systems StudyBuddy

## Technik

Das System besteht aus mehrere Schichten bzw. Komponenten:

* Ein RESTful-Service auf Basis von ASP.Net Core
  * Das Projekt findet sich im Ordner StudyBuddy.Api
  * Das Datenmodell findet sich im Ordner StudyBuddy.Model
  * Die Businesslogik findet sich im Ordner StudyBuddy.BusinessLogic
  * Die Persistenzschicht mit diversen Repositories findet sich im Ordner StudyBuddy.Persistence
  * Die Daten werden in einer PostgreSQL-Datenbank gespeichert.

* Ein Backend auf Basis von Angular
  * Hier wird der RESRful-Service benutzt
  * Das Projekt befindet sich im Ordner StudyBuddy.Backend

* Eine mobile App auf Basis von Xamarin.Forms
  * Die Projekte finden sich in den Ordnern, die mit StudyBuddy.App beginnen.

## Revisionen

### Backend 0.0.9

* Das ganze Konzept der Teams wurde komplett entfernt.
  * Teams sind aus einigen Gründen unpraktisch. Wenn später Neuigkeiten angezeigt werden sollen,
    müsste man das über alle Teams machen, in dem ein Nutzer Mitglied ist. Auch die Auswahl, welcher
    Nutzer aus welchen Teams eine Herausforderung bestätigen soll, ist wenig intuitiv.
  * Entsprechend habe ich Teams durch eine flache Freundschaftsbeziehung ersetzt.
  * Eine Freundesbeziehungen zwischen Nutzern ist unidirektional,
    ist also Nutzer1 Freund von Nutzer2, ist auch Nutzer2 Freund von Nutzer1.
  * Um das alles abzubilden, wurde das UserRepository angepasst 
    und die entsprechenden Methoden in der API hinzugefügt.
  * Alles was mit Teams zu tun hatte, wurde entfernt.
  * Im Backend kann ein Admin nun beim Bearbeiten eines Nutzers direkt Freunde verwalten.

* Es wurde begonnen, Anfragen umzusetzen.
  * Ein Nutzer kann aus unterschiedlichen Gründen Anfragen an andere Nutzer stellen, zB weil 
    er Freund sein will, oder weil er eine Bestätigung für eine Herausforderung braucht.
  * Eine entsprechende Modellklasse und das passende Repository wurde in der API implementiert.
  * Im Backend kann ein Admin solche Anfragen nun erstellen, löschen und bestätigen.
  * je nachdem, wofür die Anfrage war, wird durch die Bestätigung einer 
    Anfrage ein anderes Ergebnis erzeugt. Handelte es sich um eine Freundschaftsanfrage,
    ist anschließend die Freundschaftsbeziehung aufgebaut.
  * Es fehlt noch die Möglichkeit, Anfragen abzulehnen.

* Neuer Layer im Backend: BusinessLayer
  * Anfragen zu akzeptieren ist schon etwas komplexer. Solcher Code sollte dann aber nicht
    in den Controllern der API zu finden sein. Entsprechend wurde in der API 
    ein neuer Layer eingeführt: BusinessLogic.
  * In Zukunft sollte ausschließlich in diesem Layer die Nutzerrechte überprüft werden.
    Aktuell geschieht das noch zum großen Teil in den Controllern durch
    verschiedene Attribute, zB [IsAdmin] oder [IsLoggedIn]

### Backend 0.0.8

* Die persönlichen Einstellungen können nun geändert werden.

* Bei den Herausforderungen, Abzeichen und Teams kann der Admin nun die Besitzer ändern

### Backend 0.0.7

* Abzeichen können nun komplett erstellt werden.
  * In diesem Zug wurde auch eine neue Komponente erstellet, um Herausforderungen auszuwählen
  * Das Konzept wurde dann auch gleich für die Auswahl von Benutzern von Teams übernommen.
  * In der API wurde noch ein Bug gefixed, der verhindert hat, 
    dass man bei Abzeichen 100% bei der Abdeckung von Herausforderungen einstellen konnte.

* Es wurde begonnen, die Passwort-Reset-Mail zu implementieren.
  * Dazu braucht es aber in der API einiges an Infrastruktur, um Mails verschicken zu können.
  * Aktuell geht es da leider nicht weiter, weil mir an der HSHL nur ein Exchange-Server bekannt ist
    und der Versand aus Asp.Net Core lediglich mit SMTP leicht umsetzbar ist.

### Backend 0.0.6

* Bei den Herausforderungen sind zwei neue Attribute hinzugekommen: 
  * "Gültig für Studiengang" und "Gültig für Studierende eingeschrieben im Semester".
  * Damit soll man steuern können, welche Studierende die Herausforderung sehen sollen.
  * Entsprechend musste auch die Modell-Klasse Challenge und die DB-Struktur der Tabelle "challenges" angepasst werden.
  * Damit bei solchen Änderungen nicht immer die ganze Datenbank neu aufgesetzt werden muss, 
    wurde eine Art Migrationssystem in den Repositories der API eingebaut.

* Es wurde der Anfang gemacht für die Erstellung/Bearbeitung von Abzeichen
  * Die Komponenten GameBadgeEdit und GameBadgeList sind erstellt, letztere ist fast fertig.
  * Die Modellklasse "GameBadge" ist eingebaut und der Service "GameBadgeService" wurde begonnen.

* Einige Bugfixes, hauptsächlich in der RESTful-Api
  * Die Methoden der Repositories sind erstmal alle synchronized, 
    weil durch die parallel ablaufenden Requests teilweise komische Dinge passiert sind.

### Backend 0.0.5

* Von Herausforderungen können Serien erstellt werden.
* Herausforderungen können mit einer Volltextsuche gefiltert werden.