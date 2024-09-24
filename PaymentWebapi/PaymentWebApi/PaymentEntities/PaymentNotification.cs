namespace PaymentWebApi.PaymentEntities
{
    public class Data
    {
        public string id { get; set; }
        public Data(string id)
        {
            this.id = id;
        }
    }

    public class PaymentNotification
    {
        public string action { get; set; }
        
        public string api_version { get; set; }

        public Data data { get; set; }

        public string date_created { get; set; }

        public long id { get; set; }

        public bool live_mode { get; set; }

        public string type { get; set; }

        public string user_id { get; set; }

        public PaymentNotification(string action, string api_version, Data data, string date_created, long id, bool live_mode, string type, string user_id)
        {
            this.action = action;
            this.api_version = api_version;
            this.data = data;
            this.date_created = date_created;
            this.id = id;
            this.live_mode = live_mode;
            this.type = type;
            this.user_id = user_id;
        }
    }
}
