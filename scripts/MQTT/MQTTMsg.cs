

public class MQTTMsg
{
    public string Topic;
    public string Payload;

    public MQTTMsg(string Topic,string Payload)
    {
        this.Topic = Topic;
        this.Payload = Payload;
    }
}
