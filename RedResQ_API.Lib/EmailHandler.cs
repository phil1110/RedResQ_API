using System.Net;
using System.Net.Mail;
using System.Text;

namespace RedResQ_API.Lib
{
    internal static class EmailHandler
    {
        private static SmtpClient _client = new SmtpClient("smtp.gmail.com", 587)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("mail.redresq@gmail.com", "abkxzhwhntqevroy"),
            EnableSsl = true
        };

        public static string SendEmail(string recipient, string content)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress("mail.redresq@gmail.com"),
                    Subject = $"{DateTime.Now.ToString("dd. MMMM yyyy")}: RedResQ Password Request",
                    Body = content,
                    IsBodyHtml = true,
                };
                message.To.Add(recipient);

                _client.Send(message);
            }
            catch (Exception e)
            {
                return "!ERROR: " + e.Message;
            }

            return "E-Mail was sent successfully!";
        }

        public static string GetEmail(int confirmationCode)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Good Day!");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("This E-Mail contains the confirmation Code to reset your password in the RedResQ-App.");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine($"Here is your confirmation Code: {confirmationCode.ToString("D6")}");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("Please watch out, as the code is valid for 10 minutes only!");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("If it was not you who initiated the reset request, please reach out to us.");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("Cheers,");
            sb.AppendLine("<br />");
            sb.AppendLine("The RedResQ-Team");

            return sb.ToString();
        }
    }
}
