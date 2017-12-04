namespace ServiceUtil.Email
{
    public interface IEmailConfig
    {
        string FromAddress { get; set; }
        string FromName { get; set; }
        string LocalDomain { get; set; }
        string MailServerAddress { get; set; }
        int MailServerPort { get; set; }
        string UserId { get; set; }
        string UserPassword { get; set; }
    }
}