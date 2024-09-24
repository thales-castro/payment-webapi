namespace PaymentWebApi.PaymentEntities
{
    public class TopicMessage
    {
        public string resource { get; set; }

        public string topic { get; set; }

        public TopicMessage(string resource, string topic)
        {
            this.resource = resource;
            this.topic = topic;
        }
    }
}
