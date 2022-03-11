namespace Neutralize.Emails
{
    public class EmailSenderInput
    {
        public bool SendForEmailOnOptions { get; set; }
        
        public string EmailFrom { get; set; }
        public string NameEmailFrom { get; set; }

        public string EmailTo { get; set; }
        public string NameEmailTo { get; set; }

        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainTextContent { get; set; }

        public static EmailSenderInput Create() => new(); 

        public EmailSenderInput SetSendForEmailOnOptions(bool value = true)
        {
            SendForEmailOnOptions = value;
            return this;
        }
        
        public EmailSenderInput SetEmailFrom(string emailFrom, string nameEmailFrom)
        {
            EmailFrom = emailFrom;
            NameEmailFrom = nameEmailFrom;
            return this;
        }
        
        public EmailSenderInput SetEmailTo(string emailTo, string nameEmailTo)
        {
            EmailTo = emailTo;
            NameEmailTo = nameEmailTo;
            return this;
        }
        
        public EmailSenderInput SetSubject(string subject)
        {
            Subject = subject;
            return this;
        }
        
        public EmailSenderInput SetHtmlContent(string htmlContent)
        {
            HtmlContent = htmlContent;
            return this;
        }
        
        public EmailSenderInput SetPlainTextContent(string plainTextContent)
        {
            PlainTextContent = plainTextContent;
            return this;
        }
    }
}