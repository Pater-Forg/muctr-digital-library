using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace MDLibrary.Services
{
	public class EmailService
	{
		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var emailMessage = new MimeMessage();

			emailMessage.From.Add(new MailboxAddress("Администрация сайта MDLibrary", "admin@mdlibrary.com"));
			emailMessage.To.Add(new MailboxAddress("", email));
			emailMessage.Subject = subject;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message
			};

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync("smtp.mdlibrary.com", 465, true);
				await client.AuthenticateAsync("admin@mdlibrary.com", "password");
				await client.SendAsync(emailMessage);

				await client.DisconnectAsync(true);
			}
		}
	}
}
