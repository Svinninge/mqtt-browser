# 🌐 MQTT Browser Dashboard

En modern webbbaserad dashboard för att övervaka och visualisera MQTT-meddelanden från en MQTT-broker. Perfekt för IoT-projekt, hemautomation och motorhemmet-telemetri.

## ✨ Features

- **Real-time MQTT-monitoring** - Lyssnar på alla MQTT-topics (`#`) och visar meddelanden i realtid
- **Interaktiv dashboard** - Snygga kort för varje topic med automatisk uppdatering
- **Intelligenta ikoner** - Automatisk ikonisering baserat på topicnamn (temperatur 🌡️, alarm 🚨, GPS 📍, etc.)
- **GPS-visualisering** - Integrerad Leaflet-karta för att visa motorhemmets GPS-position
- **Responsiv design** - Fungerar perfekt på desktop, tablet och mobil
- **JSON-formatering** - Automatisk formatering av komplexa JSON-värden
- **Anslutningsstatus** - Visuell indikator för MQTT-anslutningsstatus

## 🚀 Kom igång

### Förutsättningar

- .NET 9.0 SDK eller senare
- Tillgång till en MQTT-broker (med användarnamn och lösenord)

### Installation

```bash
# Klona repot
git clone https://github.com/YOUR_USERNAME/mqtt-browser.git
cd mqtt-browser

# Installera dependencies
dotnet restore
```

### Konfiguration

Uppdatera `WebApplication1/appsettings.json` med din MQTT-broker-information:

```json
{
  "Mqtt": {
    "Broker": "din-mqtt-broker.com",
    "Port": 1883,
    "Username": "din-username",
    "Password": "din-password"
  }
}
```

> ⚠️ **Säkerhet:** För utveckling, använd `appsettings.Development.json` (ignoreras av Git). För produktion, använd miljövariabler.

### Kör projektet

```bash
cd WebApplication1
dotnet run
```

Navigera till `http://localhost:5000` (eller den port som visas i konsolen).

## 📊 Arkitektur

### Backend (C# / ASP.NET Core 9.0)

- **Program.cs** - Huvudapplikation
  - Anslutning till MQTT-broker
  - Lagring av MQTT-meddelanden i minne
  - API-endpoint `/api/mqtt-data` för att hämta alla meddelanden

### Frontend (HTML/CSS/JavaScript)

- **wwwroot/index.html** - Komplett dashboard
  - Real-time uppdatering från backend
  - Leaflet-kartintegration
  - Dynamisk rendering av MQTT-data
  - HTML-escaping för säkerhet

### Dependencies

- **MQTTnet** v4.3.3+ - MQTT-klientbibliotek
- **Leaflet.js** - Kartvisualisering (via CDN)
- **OpenStreetMap** - Kartdata (via CDN)

## 🔧 API-endpoints

### GET /api/mqtt-data
Returnerar alla insamlade MQTT-meddelanden som JSON.

**Exempel-response:**
```json
{
  "motorhome/rutx50": "{\"GPS\":{\"latitude\":59.446927,...}}",
  "homeassistant/status": "online",
  "Temperatur Åkersberga": "-3.8",
  "Current price": "1.59"
}
```

## 🛡️ Säkerhet

- ✅ MQTT-autentiseringsuppgifter lagras i konfigurationsfiler (ej hårdkodade)
- ✅ `.gitignore` skyddar känsliga filer från versionskontroll
- ✅ HTML-escaping förhindrar XSS-attacker
- ⚠️ Ingen autentisering på webbgränssnittet (rekommenderas att lägga till för produktion)
- ⚠️ Ingen persistent datalagring (data lagras endast i minne)

## 📝 Framtida förbättringar

- [ ] Persistent datalagring (databas)
- [ ] Autentisering för webbgränssnittet (JWT/OAuth)
- [ ] Historik och grafer för MQTT-data
- [ ] Konfigurerbara uppdateringsintervall
- [ ] Topic-filtrering och sök
- [ ] MQTT-publisering från dashboard
- [ ] Dark mode
- [ ] Docker-stöd
- [ ] Automatisk reconnection vid anslutningsfel

## 🐛 Kända problem

- MQTTnet-paketet uppdatering finns tillgänglig (4.3.3.952 är installerat, 4.3.3.943 efterfrågades)
- `MqttApplicationMessage.Payload` är obsolet - bör uppdateras till `PayloadSegment`

## 📄 Licens

MIT License - Se LICENSE-filen för detaljer.

## 👨‍💻 Bidrag

Bidrag är välkomna! Öppna en Issue eller Pull Request.

---

Gjord med ❤️ för IoT-entusiaster
