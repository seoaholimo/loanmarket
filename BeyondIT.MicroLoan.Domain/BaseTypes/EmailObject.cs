using System;
using System.Collections.Generic;
using System.Text;

namespace BeyondIT.MicroLoan.Domain.BaseTypes
 {
        public class EmailObject
        {
            public EmailObject()
            {
                Attachments = new List<EmailAttachment>();
            }

            public string From { get; set; }
            public string To { get; set; }
            public string Cc { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public List<EmailAttachment> Attachments { get; }
        }

        public class EmailAttachment
        {
            public EmailAttachment(byte[] fileBytes, string fileName, string mimeType)
            {
                FileBytes = fileBytes;
                FileName = fileName;
                MimeType = mimeType;
            }

            public byte[] FileBytes { get; }
            public string FileName { get; }
            public string MimeType { get; }
        }
  }


