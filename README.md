# Dies ist der Quellcode des Gamification-Systems ~~StudyBuddy~~ Gameucation.

Infos zum System finden sich [hier](https://gameucation.eu).

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
