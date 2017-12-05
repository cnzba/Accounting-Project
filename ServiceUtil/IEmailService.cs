using System.Threading.Tasks;

namespace ServiceUtil.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmail(IEmailConfig emailConfig, IEmail email);
        
    }
}