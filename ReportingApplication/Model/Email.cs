namespace ReportingApplication.Model
{
    public class Email
    {
        public string AddFrom { get; set; }
        public string AddTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string AttachmentFile { get; set; }
        public int Port { get; set; }


    }
}
