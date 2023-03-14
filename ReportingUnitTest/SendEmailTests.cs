using Castle.Core.Smtp;
using Microsoft.Extensions.Logging;
using Moq;
using netDumbster.smtp;
using NUnit.Framework;
using ReportingApplication.Email;
using ReportingApplication.Model;

namespace ReportingApplicationTest
{
    public class SendEmailTests
    {
        private SimpleSmtpServer server;
        private int port = 15525;

        [SetUp]
        public void Setup()
        {
            server = SimpleSmtpServer.Start(port);
        }

        [TearDown]
        public void TearDown()
        {
            if (server != null)
            {
                server.Stop();
            }
        }

        [Test]
        public void SendMailTest()
        {
            var mock = new Mock<ILogger<EmailService>>();
            var logger = mock.Object;
            IEmailService sender = new EmailService(logger);
            Email email = new Email
            {
                Host = "localhost",
                Port = port,
                AddFrom = "sender@here.com",
                AddTo = "receiver@there.com",
                Subject = "This is the subject",
                Body = "<h2>This is the body.</h2>"
            };

            var isSuccess = sender.SendEmail(email, false);

            Assert.IsTrue(isSuccess);
            Assert.AreEqual(1, server.ReceivedEmailCount);

            SmtpMessage mail = server.ReceivedEmail.First();

            Assert.AreEqual("sender@here.com", mail.FromAddress.Address);
            Assert.AreEqual("receiver@there.com", mail.ToAddresses.First().Address);
            Assert.AreEqual("This is the subject", mail.Subject);
            Assert.AreEqual("<h2>This is the body.</h2>", mail.MessageParts[0].BodyData);
        }
    }
}
