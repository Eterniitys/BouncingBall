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

namespace ObjectLibrary {
	class MqttWrapper {
		/// <summary>
		/// reflect the topics list
		/// </summary>
		public enum Topic {
			BALL_POS,
			NEW_WALL,
			BUILD_WALL,
			TABS_ID_POS,
			TABS_ID_ANG
		}

		/// <summary>
		/// Return a list of all used topics
		/// </summary>
		/// <returns></returns>
		internal static string[] GetFullTopicList() {
			string[] lstTopics = {
				"ball/pos",
				"wall/new",
				"wall/build",
				"tablet/+/pos",
				"tablet/+/angle",
			};
			return lstTopics;
		}
		/// <summary>
		/// Return the list of topic which is subscribed by the client
		/// </summary>
		/// <returns></returns>
		internal static string[] GetClientTopicList() {
			string[] lstTopics = {
				"ball/pos",
				"wall/new",
				"wall/build"
			};
			return lstTopics;
		}

		/// <summary>
		/// Create an instance of MQTT broker
		/// </summary>
		/// <returns></returns>
		public static IMqttServer CreateBroker() {
			return new MqttFactory().CreateMqttServer();
		}

		/// <summary>
		/// Start the broker with the choosen port
		/// </summary>
		/// <param name="mqttServer"></param>
		/// <param name="port"></param>
		public static async void StartMqttBroker(IMqttServer mqttServer, int port = 1883) {
			var optionBuilder = new MqttServerOptionsBuilder()
				.WithConnectionBacklog(100)
				.WithDefaultEndpointPort(port);

			await mqttServer.StartAsync(optionBuilder.Build());
		}

		/// <summary>
		/// Create an instance MQTT client
		/// </summary>
		/// <returns></returns>
		public static IMqttClient CreateClient() {
			var factory = new MqttFactory();
			IMqttClient mqttClient = factory.CreateMqttClient();

			mqttClient.UseApplicationMessageReceivedHandler(e => {});

			return mqttClient;
		}

		/// <summary>
		/// Make a client Subscribe to all available topic in <see cref="Topic"/>
		/// </summary>
		/// <param name="mqttClient"></param>
		public static void SetClientSubs(IMqttClient mqttClient) {
			mqttClient.UseConnectedHandler(async e => {
				foreach (string topic in GetClientTopicList()) {
					await mqttClient.SubscribeAsync(new MqttTopicFilter() { Topic = topic });
				}
			});
		}

		/// <summary>
		/// Connect a MQTT client to a MQTT broker
		/// </summary>
		/// <param name="mqttClient"></param>
		/// <param name="username"></param>
		/// <param name="url"></param>
		/// <param name="port"></param>
		public static async void ConnectClient(IMqttClient mqttClient, string username, string url, int port = 1883){
			var options = new MqttClientOptionsBuilder()
				.WithClientId(username)
				.WithTcpServer(url)
				.Build();
				var x = await mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);
		}

		/// <summary>
		/// Send a message in a topic
		/// </summary>
		/// <param name="mqttClient"></param>
		/// <param name="topic">The choosen topic</param>
		/// <param name="text">The message</param>
		/// <param name="retainFlag">Set the retain flag</param>
		public static async void SendMqttMessageTo(IMqttClient mqttClient, string topic, string text, bool retainFlag = false) {
			var message = new MqttApplicationMessageBuilder()
				.WithTopic(topic)
				.WithPayload(text)
				.WithExactlyOnceQoS()
				.WithRetainFlag(retainFlag)
				.Build();
			if (mqttClient.IsConnected) {
				try {
					await mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
				} catch { }
			}
		}
		/// <summary>
		/// Send a message in a topic
		/// </summary>
		/// <param name="mqttBroker"></param>
		/// <param name="topic">The choosen topic</param>
		/// <param name="text">The message</param>
		/// <param name="retainFlag">Set the retain flag</param>
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

