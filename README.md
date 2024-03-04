# Team03_SmartHome

Team 03:
Philipp Ehinger (7412015), Niklas Gottermann (9437344), Robin Hassler (1456701)
Thema 1 - Php / ASP.net core/ MongoDB

# Wichtige Punke
 - Bitte verwenden Sie die compose.yml im root-Ordner des Projekts für ihre Tests.
 - Wir verwenden für die Authentifizierung Bearer Tokens
    - Zur Authentifizierung kann der Endpunkt localhost:xxxx/auth/register zum anlegen und der Endpunkt localhost:xxxx/auth/login zum generieren des Tokens verwendet werden.
    - xxxx = Port
    - Im Frontend existieren passende Formulare zum Anmelden und Registrieren.
 - Standartmäßig ist nur der Port für das Frontend published. 
 - Der Port für die API (mainunit) sollte nur zu testzwecken published werden.
 - Da PHP serverseitiges Rendering verwendet, ist über den Browser nur die Kommunikation zwischen Frontend und Browser sichtbar. Alle anderen Requesets laufen innerhalb des Docker-Netzwerk ab.
 - Im Frontend steht ein Button zum anlegen von Dummy-Daten bereit. Dieser kann nur einmalig betätigt werden.