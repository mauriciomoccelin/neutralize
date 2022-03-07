namespace Neutralize.SendGrid
{
    public interface ISendGridEmailSenderOption
    {
        public string GetApiKey();
        public void SetApiKey(string value);
    }
}