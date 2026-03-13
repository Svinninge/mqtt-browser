using MQTTnet;
using MQTTnet.Client;
using System.Collections.Generic;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

// Open browser automatically when app starts
var startupCompleted = false;
var startupLock = new object();

// Load MQTT configuration from appsettings
var mqttConfig = app.Configuration.GetSection("Mqtt");
var mqttBroker = mqttConfig["Broker"] ?? "localhost";
var mqttPort = int.Parse(mqttConfig["Port"] ?? "1883");
var mqttUser = mqttConfig["Username"] ?? "";
var mqttPassword = mqttConfig["Password"] ?? "";

// Store MQTT data in memory
var mqttData = new Dictionary<string, string>();
var dataLock = new object();

// Function to connect to MQTT broker
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
            Console.WriteLine("✓ Connected to MQTT broker!");
            await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("#").Build());
        };

        client.DisconnectedAsync += _ =>
        {
            Console.WriteLine("❌ Disconnected from MQTT broker");
            return Task.CompletedTask;
        };

        await client.ConnectAsync(options, System.Threading.CancellationToken.None);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Connection error: {ex.Message}");
    }
}

// Start MQTT connection in the background
_ = ConnectToMqtt();

// API endpoint to retrieve MQTT data
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

app.Lifetime.ApplicationStarted.Register(() =>
{
    lock (startupLock)
    {
        if (!startupCompleted)
        {
            startupCompleted = true;
            // Open browser after a short delay to ensure server is ready
            Task.Delay(1000).ContinueWith(_ =>
            {
                try
                {
                    var url = "http://localhost:5187";
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not open browser: {ex.Message}");
                }
            });
        }
    }
});

app.Run();
