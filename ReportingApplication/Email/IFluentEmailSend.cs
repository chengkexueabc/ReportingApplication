using System.Threading.Tasks;

namespace ReportingApplication.Email
{
    public interface IFluentEmailSend
    {
        Task<bool> SendUsingTemplate(string to, string subject, EmailTemplate template, object model);
    }

    public enum EmailTemplate
    {
        EmailConfirmation,
        ChangeEmail
    }
}
