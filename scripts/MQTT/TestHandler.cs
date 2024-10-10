using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using Newtonsoft.Json;

public class TestHandler : MQTTMsgHandler
{

    public VideoPlayerCtrl videoPlayerCtrl;

    public VideoPlayer vp;
    public override Task Cb(MqttApplicationMessageReceivedEventArgs e)
    {
        string topic = e.ApplicationMessage.Topic;
        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        MQTTMsg msg = new(topic, payload);
        MQTTStore.mqttMsg.Add(msg);

        return Task.CompletedTask;
    }

    private void Update()
    {

        if (MQTTStore.mqttMsg.Count > 0)
        {
            foreach (var msg in MQTTStore.mqttMsg)
            {
                if (msg.Topic == "getVideoState")
                {
                    continue;
                }
                else if (msg.Topic == "tech")
                {
                    switch (msg.Payload)
                    {
                        case "play":
                            {
                                videoPlayerCtrl.CtrlVideoStatus();
                                break;
                            }
                        case "pause":
                            {
                                videoPlayerCtrl.CtrlVideoPause();
                                break;
                            }
                        case "loop":
                            {
                                Debug.Log(3);
                                videoPlayerCtrl.CtrlLoop();
                                break;
                            }
                        case "mute":
                            {
                                Debug.Log(4);
                                videoPlayerCtrl.CtrlMute();
                                break;
                            }
                    }
                }

            }

            // 构建当前状态mqtt消息
            string msgStr = JsonConvert.SerializeObject(new State()
            {
                isMute = videoPlayerCtrl.IsMute,
                isLoop = videoPlayerCtrl.IsLoop,
                target = "tech"
            });
            var stateMsg = new MqttApplicationMessageBuilder()
                    .WithTopic("state")
                    .WithPayload(msgStr)
                    .Build();

            MQTTStore.mqttClient.PublishAsync(stateMsg);
            MQTTStore.mqttMsg.Clear();

        }

    }
}
