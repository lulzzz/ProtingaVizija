﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;

namespace NotificationService
{
    public class Mail
    {
        private static readonly MailMessage mail;
        private static readonly SmtpClient smtpServer;

        static Mail()
        {
            mail = new MailMessage();
            mail.From = new MailAddress(Credentials.mailAddress);

            smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Credentials = new System.Net.NetworkCredential(Credentials.mailAddress, Credentials.mailPassword);
            smtpServer.Port = 587;
            smtpServer.EnableSsl = true;
        }

        /// <summary>
        /// Sends a mail message
        /// </summary>
        /// <param name="recipientMail">Recipient's mail</param>
        /// <param name="subject">Subject of the mail</param>
        /// <param name="body">Message</param>
        /// <param name="pictureBytes">Picture to send in byte format(optional)</param>
        /// <returns>If message is sent, returns a guid. Else, returns null</returns>
        public static string SendMail(string recipientMail, string subject, string body, byte[] pictureBytes = null)
        {
            try
            {
                mail.To.Add(recipientMail);
                mail.Subject = subject;
                mail.Body = body;

                if (pictureBytes != null)
                {
                    Bitmap picture = new Bitmap(new MemoryStream(pictureBytes));

                    var stream = new MemoryStream();
                    picture.Save(stream, ImageFormat.Jpeg);
                    stream.Position = 0;

                    mail.Attachments.Add(new Attachment(stream, "MissingPersonImage.jpeg"));
                }

                smtpServer.Send(mail);

                return Guid.NewGuid().ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}