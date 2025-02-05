using MailKit.Net.Smtp;
using MimeKit;
using SinetEmployeeLeave.Models;
using System.Threading.Tasks;


namespace SinetEmployeeLeave.Service
{
    
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Leave Management System", "admin@yourdomain.com"));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = message };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.your-email-provider.com", 587, false);
                await smtp.AuthenticateAsync("admin@yourdomain.com", "yourpassword");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }

        //Modify EmailService to Notify Employees
        public async Task SendApprovalNotificationAsync(string toEmail, string status, LeaveRequest request)
        {
            var message = $"Your leave request from {request.StartDate} to {request.EndDate} has been {status}.";

            await SendEmailAsync(toEmail, "Leave Request Update", message);
        }

    }

}
