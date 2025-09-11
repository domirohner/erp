# ERP

## Situationsanalyse

Viele KMU benötigen ERP-Funktionen, können jedoch teure Systeme wie SAP oder Oracle nicht finanzieren.

In dieser Arbeit geht es darum ein ERPForAll zu erstellen.
Dies sollte diese Lücke mit einem Freemium-Modell schliessen:

- Gratis: Finanzmodul als Einstieg
- Kostenpflichtig: Lager- und Einkaufsmodul, später erweiterbar

Die Module sollten folgende Eigenschaten beinhalten:

| Modul             | Erforderliche Eigenschaften                                                              | Optionale Eigenschaften                     |
| ----------------- | ---------------------------------------------------------------------------------------- | ------------------------------------------- |
| **Finanzmodul**   | - Kreditoren verwalten<br>- Debitoren verwalten<br>- Umsatz pro Artikel erstellen können | - Mahnliste für säumige Debitoren           |
| **Lagermodul**    | - Lagerorte definieren<br>- Lagermengen pro Artikel verwalten                            | - ABC-Liste für gelagerte Artikel erstellen |
| **Einkaufsmodul** | - Lieferanten (=Kreditoren) verwalten<br>- Bestellungen von Artikeln verwalten           | - ABC-Liste der Lieferanten                 |

Ziel ist es, ein allgemeingültiges ERM zu entwerfen, dieses im SQL-Server umzusetzen und die resultierende Applikation anhand definierter Testszenarien zu prüfen.

## Planung

Zu Beginn unseres Projektes haben wir alle Tätigkeiten aufgelistet und die dafür benötigte Zeit geschätzt.
In der nachfolgenden Tabelle sind alle geschätzten und effektiv geleisteten Stunden ersichtlich:

![Zeitplan](Zeitplan.png)

## Kontextdiagramme

Nach Analyse der Situation und Ausgangslage haben wir zuerst ein Kontextdiagramm mit allen Modulen erstellt:

![Kontextdiagramm_Grob](Kontextdiagramm_Grob.png)

Da dieses Kontextdiagramm sehr grob ist, haben wir für alle Module auch noch ein eigenes Kontextdiagramm erstellt.

### Kontextdiagramm Finanzmodul

![Kontextdiagramm_Finanzmodul](Kontextdiagramm_Finanzmodul.png)

### Kontextdiagramm Einkaufsmodul

![Kontextdiagramm_Einkaufmodul](Kontextdiagramm_Einkaufmodul.png)

### Kontextdiagramm Lagermodul

![Kontextdiagramm_Lagermodul](Kontextdiagramm_Lagermodul.png)

## Prozesse

Wir haben uns auch noch zwei Prozesse genauer angeschaut um die Szenarien und Module besser zu verstehen.

Prozess "Bestellt Artikel":

![Prozess_bestellt_Artikel](Prozess_bestellt_Artikel.png)

Prozess "Bestellt Artikel":

![Prozess_erstellt_Rechnung](Prozess_erstellt_Rechnung.png)

## Anforderungsanalyse

Auch die Anfoderungen und Risiken für unser Projekt haben wir aufgelistet:

![Anforderungsanalyse](Anforderungsanalyse.png)

Das grösste Risiko für unser Projekt wäre, wenn Daten nicht gespeichert würden oder diese falsch abgelegt würden. Dies wäre kritisch, da somit Daten verloren gehen würden.

## ERM/ERD

Da wir dann ein gutes Verständniss hätten der jeweiligen Module konnten wir ein ERD für unsere Datenbank erstellen:

![ERD](ERD.png)

Anschlissend auch das ERM:

![ERM](ERM.png)

## Korrelationsmatrix

Durch das ERM konnte auch eine Korrelationsmatrix erstellt werden:

![Korrelationsmatrix](Korrelationsmatrix.png)

## Klassendiagramm

Auch eine Klassendiagramm haben wir noch erstellt:

![Klassendiagramm](Klassendiagramm.png)

## Testfälle

Folgende Testfälle wurden definiert und abgearbeitet:

![Testfälle](Testfaelle.png)

## Realisation

In unserer Applikation haben wir zuerst das UI mit Windows Forms erstellt:

![UI](UI.png)

Dabei haben wir uns für drei Tabs entschieden:

- Finanzmodul
- Lagermodul
- Einkaufsmodul

Die Datenbank wurde über SQL Server Management Studio von Hand erstellt:

![DB](Datenbank.png)

Sämtliche DB Skripts sind [hier](/src/DB_Scripts/) zu finden. Diese wurden nachfolgend automatisch via Studio erstellt und abgelegt.

Nach der Erstellung der Datenbank haben wir mit der Programmierung begonnen. Dabei mussten wir zuerst die Verbindung zur Datenbank herstellen, damit wir in diese schreiben können.

Zu Beginn der Programmierung hatten wir den gesamten Code in einer Daten. Da dies schnell unübersichtlich wurde, mussten wir den Code aufteilen:

![Aufteilung_Code](Aufteilung_ERP_Code.png)

Die einzelnen Klassen haben wir im Models Ordner abgelegt. Die SQL Queries im Data Ordner, auch andere Helper, wie ein Helper um in die Datenbank zu schreiben, wurde in diesem Ordner abgelegt.

Dies hat uns geholfen, den Code strukturierter zu halten und es konnte einfacher parallel am Code gearbeitet werden.

Wir haben kein Usermanagement implementiert, da je nach Lizen die einzelnen Module (Tabs) freigeschaltet werden sollen. Die anderen Module werden ausgegraut und können nicht verwendet werden.
Zudem hätten wir wir die Sicherheit für das Abspeichern der Passwörter der Benutzer nicht sicherstellen können.

## Durchführung Tests

Alle Tests wurden erfolgreich durchgeführt:

![Tests_Durchfuehrung](Tests_Durchfuehrung.png)

Somit können wir sicher sein, dass unsere Applikation so läuft, wie sie sollte.

## Fazit

Das Projekt zur Entwicklung eines ERPs war insgesamt erfolgreich und sehr lehrreich.
Die Anwendung der im Unterricht behandelten Konzepte, darunter die Erstellung von Diagrammen wie ERM, ERD und Kollerationsmatrix konnten anhand eines praktischen Beispiels angewandt werden.
Dies war sehr nützlich zur Festigung des gelernten Schulinhaltes.

Während der Entwicklung hatten wir zuerst einige Probleme mit dem Schreiben in die Datenbank. Als diese Probleme dann aber gelöst waren, konnten wir sehr schnell Fortschritte mit der Programmierung machen.
Dies verdeutlicht die Herausforderungen in der Softwareentwicklung, insbesondere in der Fehleranalyse und Behebung.
Alle wesentlichen Funktionen konnten trotzdem erfolgreich umgesetzt werden.

Wir haben auch bemerkt, dass das gemeinsame Arbeiten an einem Projekt zum Teil schwierig war, damit nicht die Änderung des Kollegen überschrieben werden.
Dies forderte eine gute Kommunikation im Team und ständiges absprechen.

Insgesamt war das Projekt eine gute Erfahrung, die Spass gemacht hat und einen praktischen Bezug zur Theorie herstellte.
