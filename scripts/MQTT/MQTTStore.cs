using MQTTnet;
using MQTTnet.Client;
using System.Collections.Generic;


public class MQTTStore
{
    public static MqttFactory mqttFactory;
    public static IMqttClient mqttClient;
    public static List<string> mqttMsg = new();
}
