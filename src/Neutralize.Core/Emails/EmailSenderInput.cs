namespace Neutralize.Emails
{
    public class EmailSenderInput
    {
        public string EmailFrom { get; set; }
        public string NameEmailFrom { get; set; }

        public string EmailTo { get; set; }
        public string NameEmailTo { get; set; }

        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainTextContent { get; set; }
    }
}