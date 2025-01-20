
using AzureIoTServer.Models;
using HiveMQtt.Client;
using HiveMQtt.Client.Exceptions;
using HiveMQtt.Client.Options;
using HiveMQtt.MQTT5.ReasonCodes;
using HiveMQtt.MQTT5.Types;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace AzureIoTServer.Services
{
    public sealed class MQTTService(ILogger<MQTTService> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            HiveMQClientOptions mqttOptions = new HiveMQClientOptions
            {
                Host = "6ff99c4d5207411b94d3dcebbaa9e437.s1.eu.hivemq.cloud",
                Port = 8883,
                UseTLS = true,
                UserName = "rcaron",
                Password = "Apsodi81!"
            };

            HiveMQClient mqttClient = new HiveMQClient(mqttOptions);
            logger.LogInformation($"Connecting to {mqttOptions.Host} on port {mqttOptions.Port}.");

            HiveMQtt.Client.Results.ConnectResult connectionResult;
            try
            {
                connectionResult = await mqttClient.ConnectAsync().ConfigureAwait(false);
                if (connectionResult.ReasonCode == ConnAckReasonCode.Success)
                {
                    logger.LogInformation($"Connection Sucessful : {connectionResult}");
                } else
                {
                    logger.LogError($"Connection failed : {connectionResult}");
                    Environment.Exit(-1);
                }
            }
            catch(HiveMQttClientException e)
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
                dbContext.Add(new ESP32CamLogs(receivedMessage));
                dbContext.SaveChanges();
                logger.LogInformation(receivedMessage);
            };

            // Subscribe
            await mqttClient.SubscribeAsync("iotserver/commands").ConfigureAwait(false);

            // Wait till application stops
            while (!stoppingToken.IsCancellationRequested);

            //// Start Publishing...
            //try
            //{
            //    while (!stoppingToken.IsCancellationRequested)
            //    {
            //        string messageToSend = $"This is a fantastic message at {DateTime.Now}";
            //        var result = await mqttClient.PublishAsync("iotserver/message", messageToSend, QualityOfService.ExactlyOnceDelivery).ConfigureAwait(false);

                //        await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
                //    }
                //}
                //catch(OperationCanceledException)
                //{

                //}
                //catch(Exception ex)
                //{
                //    logger.LogError(ex.ToString());
                //    Environment.Exit(1);
                //}
        }
    }
}
