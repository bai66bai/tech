using MQTTnet.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MQTTMsgHandler : MonoBehaviour
{
    public List<string> SubscribeList;
    public abstract Task Cb(MqttApplicationMessageReceivedEventArgs eventArgs);

    private MQTT Mqtt;

    /// <summary>
    /// 初始化消息处理器
    /// 1.添加所有订阅
    /// 2.添加消息回调
    /// </summary>
    public async void InitHandler()
    {
        Mqtt = GetComponent<MQTT>();

        Mqtt.Client.ApplicationMessageReceivedAsync += Cb;

        MqttClientSubscribeOptionsBuilder subscribeOptionsBuilder = Mqtt.Factory.CreateSubscribeOptionsBuilder();
        foreach (var topic in SubscribeList)
        {
            subscribeOptionsBuilder = subscribeOptionsBuilder.WithTopicFilter(topic);
        }
        MqttClientSubscribeOptions subscribeOptions = subscribeOptionsBuilder.Build();
        await Mqtt.Client.SubscribeAsync(subscribeOptions, CancellationToken.None);
    }


    public async void ReInitHandler()
    {
        var client = MQTTStore.mqttClient;
        var factory = MQTTStore.mqttFactory;

        Mqtt.Client.ApplicationMessageReceivedAsync += Cb;

        MqttClientSubscribeOptionsBuilder subscribeOptionsBuilder = factory.CreateSubscribeOptionsBuilder();
        foreach (var topic in SubscribeList)
        {
            subscribeOptionsBuilder = subscribeOptionsBuilder.WithTopicFilter(topic);
        }
        MqttClientSubscribeOptions subscribeOptions = subscribeOptionsBuilder.Build();
        await Mqtt.Client.SubscribeAsync(subscribeOptions, CancellationToken.None);
    }

    public async void OnDestroy()
    {
        Mqtt.Client.ApplicationMessageReceivedAsync -= Cb;

        MqttClientUnsubscribeOptionsBuilder unsubscribeOptionsBuilder = Mqtt.Factory.CreateUnsubscribeOptionsBuilder();
        foreach (var topic in SubscribeList)
        {
            unsubscribeOptionsBuilder = unsubscribeOptionsBuilder.WithTopicFilter(topic);
        }
        MqttClientUnsubscribeOptions unsubscribeOptions = unsubscribeOptionsBuilder.Build();
        await Mqtt.Client.UnsubscribeAsync(unsubscribeOptions, CancellationToken.None);
    }
}
