using MQTTnet;
using MQTTnet.Client;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

// MQTT-konfiguration från appsettings
var mqttConfig = app.Configuration.GetSection("Mqtt");
var mqttBroker = mqttConfig["Broker"] ?? "localhost";
var mqttPort = int.Parse(mqttConfig["Port"] ?? "1883");
var mqttUser = mqttConfig["Username"] ?? "";
var mqttPassword = mqttConfig["Password"] ?? "";

// Lagrad MQTT-data
var mqttData = new Dictionary<string, string>();
var dataLock = new object();

// Funktion för att ansluta till MQTT-brokern
async Task ConnectToMqtt()
{
    try
    {
        var factory = new MqttFactory();
        var client = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(mqttBroker, mqttPort)
            .WithCredentials(mqttUser, mqttPassword)
            .Build();

        client.ApplicationMessageReceivedAsync += e =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            
            lock (dataLock)
            {
                mqttData[topic] = payload;
            }
            
            Console.WriteLine($"📨 {topic}: {payload}");
            return Task.CompletedTask;
        };

        client.ConnectedAsync += async _ =>
        {
            Console.WriteLine("✓ Ansluten till MQTT-broker!");
            await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("#").Build());
        };

        client.DisconnectedAsync += _ =>
        {
            Console.WriteLine("❌ Frånkopplad från MQTT-broker");
            return Task.CompletedTask;
        };

        await client.ConnectAsync(options, System.Threading.CancellationToken.None);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Fel vid anslutning: {ex.Message}");
    }
}

// Starta MQTT-anslutning i bakgrunden
_ = ConnectToMqtt();

// API-endpoint för att hämta MQTT-data
app.MapGet("/api/mqtt-data", () =>
{
    lock (dataLock)
    {
        return Results.Json(mqttData);
    }
});

app.MapGet("/", async (HttpContext context) =>
{
    var indexPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "index.html");
    if (File.Exists(indexPath))
    {
        var content = await File.ReadAllTextAsync(indexPath);
        return Results.Content(content, "text/html");
    }
    return Results.NotFound();
});

app.Run();
