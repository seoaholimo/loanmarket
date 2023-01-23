using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using System;
using System.IO;
using System.Net.Mail;

namespace BeyondIT.MicroLoan.Infrastuture.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpClientSettings _smtpClientSettings;

        public EmailService(IAppSettingsAccessor configurationService)
        {
            _smtpClientSettings = configurationService.AppSettings.Data.SmtpClientSettings;

            _smtpClient = new SmtpClient
            {
              
            };

            if (_smtpClientSettings.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                _smtpClient.PickupDirectoryLocation = GetDirectoryFullPath(_smtpClientSettings.PickupDirectoryLocation);
            }
            else
            {
                _smtpClient.Host = _smtpClientSettings.Host;
                _smtpClient.Port = _smtpClientSettings.Port;
               // _smtpClient.EnableSsl = true;
                _smtpClient.Credentials =
                    new System.Net.NetworkCredential(_smtpClientSettings.UserName, _smtpClientSettings.Password);
            }
        }

        public void SendEmail(EmailObject emailObject)
        {
            var mail = new MailMessage {From = new MailAddress(_smtpClientSettings.From)};

            foreach (string emailAddress in emailObject.To.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
            {
                var mailTo = new MailAddress(emailAddress);
                mail.To.Add(mailTo);
            }

            if (!string.IsNullOrEmpty(emailObject.Cc))
            {
                foreach (string emailAddress in emailObject.Cc.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)
                )
                {
                    var ccTo = new MailAddress(emailAddress);
                    mail.CC.Add(ccTo);
                }
            }

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream ecoFleetLogoStream =
                myAssembly.GetManifestResourceStream("etl.PNG");
            string inlineLogoImageSrc = string.Empty;
            LinkedResource inlineLogo = null;
            if (ecoFleetLogoStream != null)
            {
                inlineLogo = new LinkedResource(ecoFleetLogoStream) {ContentId = Guid.NewGuid().ToString()};
                inlineLogoImageSrc = $"cid:{inlineLogo.ContentId}";
            }

            AlternateView view =
                AlternateView.CreateAlternateViewFromString(
                    emailObject.Body.Replace("[AdviceNoteLogo]", inlineLogoImageSrc), null, "text/html");
            if (inlineLogo != null && emailObject.Body.Contains("[BeyondITLogo]"))
            {
                view.LinkedResources.Add(inlineLogo);
            }

            mail.AlternateViews.Add(view);

            mail.Subject = emailObject.Subject;
            mail.Body = emailObject.Body.Replace("[BeyondITLogo]", inlineLogoImageSrc);
            mail.IsBodyHtml = true;

            foreach (EmailAttachment emailAttachment in emailObject.Attachments)
            {
                var attachment = new Attachment(new MemoryStream(emailAttachment.FileBytes), emailAttachment.MimeType)
                {
                    Name = emailAttachment.FileName
                };

                mail.Attachments.Add(attachment);
            }

            try
            {
                _smtpClient.Send(mail);
            }
            finally
            {
                foreach (Attachment attachment in mail.Attachments)
                {
                    attachment.ContentStream.Dispose();
                }
            }
        }

        private static string GetDirectoryFullPath(string relativeDirectory)
        {
            if (Directory.Exists(relativeDirectory))
            {
                return relativeDirectory;
            }
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            string directory = $"{Path.GetDirectoryName(location)}{relativeDirectory}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}