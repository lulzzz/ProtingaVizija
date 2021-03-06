﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;
using Constants;

namespace NotificationService
{
    public class Mail
    {
        private static readonly MailMessage mail;
        private static readonly SmtpClient smtpServer;

        static Mail()
        {
            try
            {
                mail = new MailMessage { From = new MailAddress(Credentials.mailAddress) };

                smtpServer = new SmtpClient("smtp.gmail.com")
                {
                    Credentials = new System.Net.NetworkCredential(Credentials.mailAddress, Credentials.mailPassword),
                    Port = 587,
                    EnableSsl = true
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        /// <summary>
        /// Sends a mail message
        /// </summary>
        /// <param name="recipientMail">Recipient's mail</param>
        /// <param name="subject">Subject of the mail</param>
        /// <param name="body">Message</param>
        /// <param name="pictureBytes">Picture to send in byte format(optional)</param>
        /// <returns>If message is sent, returns a guid. Else, returns null</returns>
        public static string SendMail(string recipientMail, string subject, string body, List<byte[]> pictureBytes = null, List<string> pictureNames = null)
        {
            try
            {
                if (mail == null)
                {
                    throw new Exception("Error while creating mail message. Please check, whether your provided data is valid");
                }

                mail.To.Add(recipientMail);
                mail.Subject = subject;
                mail.Body = body;

                for (int i = 0; i < pictureBytes.Count; i++)
                {
                    if (pictureBytes[i] != null && pictureNames[i] != null)
                    {
                        Bitmap picture = new Bitmap(new MemoryStream(pictureBytes[i]));

                        var stream = new MemoryStream();
                        picture.Save(stream, ImageFormat.Jpeg);
                        stream.Position = 0;

                        mail.Attachments.Add(new Attachment(stream, pictureNames[i]));
                    }
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