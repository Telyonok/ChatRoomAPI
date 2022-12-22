using ChatRoomAPI.Models;
using Microsoft.Extensions.Options;
using FluentEmail.Mailgun;
using FluentEmail.Core;

namespace ChatRoomAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailGunSettings _mailGunSettings;

        public EmailService(IOptions<MailGunSettings> mailGunSettings)
        {
            _mailGunSettings = mailGunSettings.Value;
        }

        public async Task SendConfirmationEmailAsync(User user)
        {
            var sender = new MailgunSender(
                _mailGunSettings.Domain, _mailGunSettings.ApiKey);
            Email.DefaultSender = sender;
            var email = Email
                .From("Administration@chatroom.com")
                .To(user.Email)
                .Subject("Email confirmation")
                .Body($"Click <a href = \"https://localhost:7225/Verifyemail/{user.VerificationData}\">here</a> to verify your email ");
            var response = await email.SendAsync();
        }
    }
}
