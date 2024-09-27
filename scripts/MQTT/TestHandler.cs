using MQTTnet.Client;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class TestHandler : MQTTMsgHandler
{

    public VideoPlayerCtrl videoPlayerCtrl;

    public VideoPlayer vp;
    public override Task Cb(MqttApplicationMessageReceivedEventArgs e)
    {
        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        MQTTStore.mqttMsg.Add(payload);

        return Task.CompletedTask;
    }

    private void Update()
    {
        if (MQTTStore.mqttMsg.Count > 0)
        {
            foreach (var msg in MQTTStore.mqttMsg)
            {
                switch (msg)
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

            MQTTStore.mqttMsg.Clear();

        }

    }
}
