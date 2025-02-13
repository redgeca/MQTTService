
using IoTFallServer.Models;
using HiveMQtt.Client;
using HiveMQtt.Client.Exceptions;
using HiveMQtt.Client.Options;
using HiveMQtt.MQTT5.ReasonCodes;
using HiveMQtt.MQTT5.Types;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Linq.Expressions;

namespace IoTFallServer.BackgroundServices.MQTT
{
    public sealed class MQTTService(
        ILogger<MQTTService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration) : BackgroundService
    {
        private HiveMQClient? mqttClient;
        private HiveMQClientOptions mqttOptions = new HiveMQClientOptions
        {
            Host = "6ff99c4d5207411b94d3dcebbaa9e437.s1.eu.hivemq.cloud",
            Port = 8883,
            UseTLS = true,
            UserName = "rcaron",
            Password = "Apsodi81!",
            AutomaticReconnect = true,
            ClientId = configuration.GetValue<string>("MQTTUniqueName"),
            KeepAlive = 60
        };

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                mqttClient = new HiveMQClient(mqttOptions);
                logger.LogInformation($"Connecting to {mqttOptions.Host} on port {mqttOptions.Port}.");

                HiveMQtt.Client.Results.ConnectResult connectionResult;
                try
                {
                    connectionResult = await mqttClient.ConnectAsync().ConfigureAwait(false);
                    if (connectionResult.ReasonCode == ConnAckReasonCode.Success)
                    {
                        logger.LogInformation($"Connection Sucessful : {connectionResult}");
                    }
                    else
                    {
                        logger.LogError($"Connection failed : {connectionResult}");
                        Environment.Exit(-1);
                    }
                }
                catch (HiveMQttClientException e)
                {
                    logger.LogError($"Error connecting : {e.Message}");
                    Environment.Exit(-1);
                }
                catch (SocketException e)
                {
                    logger.LogError($"Error connecting : {e.Message}");
                    Environment.Exit(-1);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                    Environment.Exit(1);
                }

                mqttClient.OnMessageReceived += (sender, args) =>
                {
                    string receivedMessage = args.PublishMessage.PayloadAsString;
                    using IServiceScope scope = serviceScopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<IoTDBContext>();
                    JsonObject? jsonObject = JsonNode.Parse(receivedMessage)!.AsObject();
                    //dbContext.Add(new Temperature
                    //{
                    //    dateTime = DateTime.Now,
                    //    temperature = jsonObject["temp"]!.GetValue<float>()
                    //});
                    //
                    //                    dbContext.SaveChanges();
                    logger.LogInformation(receivedMessage);
                };

                // Subscribe
                await mqttClient.SubscribeAsync("/rcaron/FallDetectServer/fallDetected", QualityOfService.AtLeastOnceDelivery).ConfigureAwait(false);

                // Wait till application stops
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                };
            }
            catch (OperationCanceledException)
            {
                if (mqttClient != null)
                {
                    await mqttClient.DisconnectAsync().ConfigureAwait(true);
                }
            }
            finally
            {
                logger.LogInformation("Ending MQTT Service task");
            }
        }
    }
}
