using ReportingApplication.Model;
using System.Collections.Generic;

namespace ReportingApplication.Email
{
    public interface IEmailService
    {
        public bool SendEmail(Model.Email email, bool enableSsl = true);


    }
}
