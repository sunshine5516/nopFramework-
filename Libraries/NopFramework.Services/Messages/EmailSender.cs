using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Domains.Messages;
using System.Net.Mail;
using System.IO;
using System.Net;

namespace NopFramework.Services.Messages
{
    public partial class EmailSender : IEmailSender
    {
        public void SendEmail(EmailAccount emailAccount, string subject, string body, string fromAddress,
            string fromName, string toAddress, string toName, string replyTo = null,
            string replyToName = null, IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            string attachmentFilePath = null, string attachmentFileName = null, int attachedDownloadId = 0,
            IDictionary<string, string> headers = null)
        {
            var message = new MailMessage();
            //from, to, reply to
            message.From = new MailAddress(fromAddress, fromName);
            message.To.Add(new MailAddress(toAddress, toName));
            if (!String.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }
            ///私密邮件
            if (bcc != null)
            {
                foreach(var address in bcc.Where(bccValue => !String.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }
            ///抄送
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !String.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }
            //内容
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key,header.Value);
                }
            }
            ///附件
            if (!String.IsNullOrEmpty(attachmentFilePath) && File.Exists(attachmentFilePath))
            {
                var attachment = new Attachment(attachmentFilePath);
                attachment.ContentDisposition.CreationDate = File.GetCreationTime(attachmentFilePath);
                attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentFilePath);
                attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(attachmentFilePath);
                if (!String.IsNullOrEmpty(attachmentFileName))
                {
                    attachment.Name = attachmentFileName;
                }
                message.Attachments.Add(attachment);
            }
            ///其他附件
            //if (attachedDownloadId > 0)
            //{
            //    var download = _downloadService.GetDownloadById(attachedDownloadId);
            //    if (download != null)
            //    {
            //        //we do not support URLs as attachments
            //        if (!download.UseDownloadUrl)
            //        {
            //            string fileName = !String.IsNullOrWhiteSpace(download.Filename) ? download.Filename : download.Id.ToString();
            //            fileName += download.Extension;


            //            var ms = new MemoryStream(download.DownloadBinary);
            //            var attachment = new Attachment(ms, fileName);
            //            //string contentType = !String.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : "application/octet-stream";
            //            //var attachment = new Attachment(ms, fileName, contentType);
            //            attachment.ContentDisposition.CreationDate = DateTime.UtcNow;
            //            attachment.ContentDisposition.ModificationDate = DateTime.UtcNow;
            //            attachment.ContentDisposition.ReadDate = DateTime.UtcNow;
            //            message.Attachments.Add(attachment);
            //        }
            //    }
            //}
            //发送电子邮件
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Post;
                smtpClient.EnableSsl = emailAccount.EnableSsl;
                smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                    CredentialCache.DefaultNetworkCredentials :
                    new NetworkCredential(emailAccount.UserName, emailAccount.Password);
                smtpClient.Send(message);
            }
                //throw new NotImplementedException();
        }
    }
}
