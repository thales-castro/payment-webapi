namespace PaymentWebApi.PaymentEntities
{
    public class Error
    {
        public string error { get; set; }
        public string message { get; set; }
        public int status { get; set; }

        public Error(string error, string message, int status)
        {
            this.error = error;
            this.message = message;
            this.status = status;
        }
    }
}
