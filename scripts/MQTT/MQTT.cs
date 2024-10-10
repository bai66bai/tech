using MQTTnet.Client;
using UnityEngine;
using MQTTnet;

public class MQTT : MonoBehaviour
{
    [HideInInspector]
    public MqttFactory Factory;
    [HideInInspector]
    public IMqttClient Client;

    public string IP = "127.0.0.1";

    private MQTTMsgHandler msgHandler;

    void Start()
    {
        msgHandler = GetComponent<MQTTMsgHandler>();
        Factory = MQTTStore.mqttFactory ?? new();
        Client = MQTTStore.mqttClient ?? Factory.CreateMqttClient();

        if (MQTTStore.mqttClient == null)
        {
            InitMqtt();
        }

        MQTTStore.mqttFactory = Factory;
        MQTTStore.mqttClient = Client;
    }

    async void InitMqtt()
    {
        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
               .WithTcpServer(IP)
               .Build();


        Client.DisconnectedAsync += async (e) =>
        {
            MqttClientConnectResult reconnectRet = await Client.ConnectAsync(mqttClientOptions);
            if (reconnectRet.ResultCode == MqttClientConnectResultCode.Success)
            {
                msgHandler.OnDestroy();
                msgHandler.ReInitHandler();
            }
        };

        MqttClientConnectResult ret = await Client.ConnectAsync(mqttClientOptions);
        if (ret.ResultCode != MqttClientConnectResultCode.Success)
        {
            Debug.LogError($"MQTT¡¨Ω” ß∞‹");
        }
        else
        {
            msgHandler.InitHandler();
        }
    }

}
