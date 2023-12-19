using UserManagement.Service.Email.Models;

namespace UserManagement.Service.Email
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}