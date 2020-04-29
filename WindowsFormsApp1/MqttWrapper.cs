using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	class MqttWrapper {
		public enum Topic {
			BALL_POS,
			NEW_WALL,
			BUILD_WALL,
			TABS_IDS,
			TABS_ID_POS,
			TABS_ID_ANG
		}

		internal static string[] getTopicList() {
			string[] lstTopics = {
				"ball/pos",
				"wall/new",
				"wall/build",
				"tablet/id",
				"tablet/id/+/pos",
				"tablet/id/+/angle",
			};
			return lstTopics;
		}

		public static IMqttServer CreateBroker() {
			return new MqttFactory().CreateMqttServer();
		}

		public static async void StartMqttBroker(IMqttServer mqttServer, int port = 1883) {
			var optionBuilder = new MqttServerOptionsBuilder()
				.WithConnectionBacklog(100)
				.WithDefaultEndpointPort(port);
			await mqttServer.StartAsync(optionBuilder.Build());
		}

		public static IMqttClient CreateClient() {
			var factory = new MqttFactory();
			IMqttClient mqttClient = factory.CreateMqttClient();

			mqttClient.UseApplicationMessageReceivedHandler(e => {
				/*
				Invoke(new Action(() => {
					lbl_status.Text = "Message received";

					label3.Text = String.Format("{0}", e.ApplicationMessage.Topic);
					label3.Text = String.Format("{0}", e.ApplicationMessage.QualityOfServiceLevel);
					label3.Text = String.Format("{0}", e.ApplicationMessage.Retain);
					label3.Text = String.Format("{0}", e.ApplicationMessage.Payload);
					String hoy = e.ApplicationMessage.ContentType;
					label3.Text = hoy;
				}));
				*/
				/*Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
				Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
				Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
				Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
				Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
				Console.WriteLine();

				Task.Run(() => mqttClient.PublishAsync("hello/world"));*/
			});

			return mqttClient;
		}

		public static void SetClientSubs(IMqttClient mqttClient) {
			mqttClient.UseConnectedHandler(async e => {
				foreach (string topic in getTopicList()) {
					await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());
				}
			});
		}

		public static async void connectClient(IMqttClient mqttClient, string username, string url, int port = 1883) {
			var options = new MqttClientOptionsBuilder()
				.WithClientId("Client1")
				.WithTcpServer(url)
				.Build();
			await mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);
		}

		public static async void SendMqttMessageTo(IMqttClient mqttClient, string topic, string text, bool retainFlag = false) {
			var message = new MqttApplicationMessageBuilder()
				.WithTopic(topic)
				.WithPayload(text)
				.WithExactlyOnceQoS()
				.WithRetainFlag(retainFlag)
				.Build();
			if (mqttClient.IsConnected) {
				await mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
			}
		}

		public static async void SendMqttMessageTo(IMqttServer mqttBroker, string topic, string text, bool retainFlag = false) {
			var message = new MqttApplicationMessageBuilder()
				.WithTopic(topic)
				.WithPayload(text)
				.WithExactlyOnceQoS()
				.WithRetainFlag(retainFlag)
				.Build();
				await mqttBroker.PublishAsync(message, System.Threading.CancellationToken.None);
		}
	}
}

