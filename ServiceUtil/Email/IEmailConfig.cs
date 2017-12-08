namespace ServiceUtil.Email
{
    public interface IEmailConfig
    {
        string FromAddress { get; set; }
        string FromName { get; set; }
        string LocalDomain { get; set; }
        string MailServerAddress { get; set; }
        string MailServerPort { get; set; }
        string UserId { get; set; }
        string UserPassword { get; set; }
    }
}