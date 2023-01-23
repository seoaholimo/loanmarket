

using BeyondIT.MicroLoan.Domain.BaseTypes;

namespace BeyondIT.MicroLoan.Infrastuture.Email
{
    public interface IEmailService
    {
        void SendEmail(EmailObject emailObject);
    }
}
