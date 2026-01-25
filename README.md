# 🌐 MQTT Browser Dashboard

A modern web-based dashboard for monitoring and visualizing MQTT messages from an MQTT broker. Perfect for IoT projects, home automation, and motorhome telemetry.

## ✨ Features

- **Real-time MQTT monitoring** - Listens to all MQTT topics (`#`) and displays messages in real-time
- **Interactive dashboard** - Beautiful cards for each topic with automatic updates
- **Smart icons** - Automatic iconification based on topic names (temperature 🌡️, alarm 🚨, GPS 📍, etc.)
- **GPS visualization** - Integrated Leaflet map to display motorhome GPS position
- **Responsive design** - Works perfectly on desktop, tablet, and mobile
- **JSON formatting** - Automatic formatting of complex JSON values
- **Connection status** - Visual indicator for MQTT connection status

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Access to an MQTT broker (with username and password)

### Installation

```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/mqtt-browser.git
cd mqtt-browser

# Install dependencies
dotnet restore
```

### Configuration

Update `WebApplication1/appsettings.json` with your MQTT broker information:

```json
{
  "Mqtt": {
    "Broker": "your-mqtt-broker.com",
    "Port": 1883,
    "Username": "your-username",
    "Password": "your-password"
  }
}
```

> ⚠️ **Security:** For development, use `appsettings.Development.json` (ignored by Git). For production, use environment variables.

### Running the Project

```bash
cd WebApplication1
dotnet run
```

Navigate to `http://localhost:5000` (or the port shown in the console).

## 📊 Architecture

### Backend (C# / ASP.NET Core 9.0)

- **Program.cs** - Main application
  - Connection to MQTT broker
  - Storage of MQTT messages in memory
  - API endpoint `/api/mqtt-data` to retrieve all messages

### Frontend (HTML/CSS/JavaScript)

- **wwwroot/index.html** - Complete dashboard
  - Real-time updates from backend
  - Leaflet map integration
  - Dynamic rendering of MQTT data
  - HTML escaping for security

### Dependencies

- **MQTTnet** v4.3.3+ - MQTT client library
- **Leaflet.js** - Map visualization (via CDN)
- **OpenStreetMap** - Map data (via CDN)

## 🔧 API Endpoints

### GET /api/mqtt-data
Returns all collected MQTT messages as JSON.

**Example response:**
```json
{
  "motorhome/rutx50": "{\"GPS\":{\"latitude\":59.446927,...}}",
  "homeassistant/status": "online",
  "Temperatur Åkersberga": "-3.8",
  "Current price": "1.59"
}
```

## 🛡️ Security

- ✅ MQTT credentials stored in configuration files (not hardcoded)
- ✅ `.gitignore` protects sensitive files from version control
- ✅ HTML escaping prevents XSS attacks
- ⚠️ No authentication on the web interface (recommended to add for production)
- ⚠️ No persistent data storage (data only stored in memory)

## 📝 Future Improvements

- [ ] Persistent data storage (database)
- [ ] Web interface authentication (JWT/OAuth)
- [ ] Data history and graphs
- [ ] Configurable update intervals
- [ ] Topic filtering and search
- [ ] MQTT publishing from dashboard
- [ ] Dark mode
- [ ] Docker support
- [ ] Automatic reconnection on connection failure

## 🐛 Known Issues

- MQTTnet package update available (4.3.3.952 installed, 4.3.3.943 requested)
- `MqttApplicationMessage.Payload` is obsolete - should be updated to `PayloadSegment`

## 📄 License

MIT License - See LICENSE file for details.

## 👨‍💻 Contributing

Contributions are welcome! Open an Issue or Pull Request.

---

Made with ❤️ for IoT enthusiasts
