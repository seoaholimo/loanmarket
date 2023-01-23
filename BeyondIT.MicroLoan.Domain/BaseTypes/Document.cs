using System;
using System.Collections.Generic;
using System.Text;

namespace BeyondIT.MicroLoan.Domain.BaseTypes
{
    
        public class Document
        {
            public string DocumentName { get; set; }
            public string DocumentDescription { get; set; }
            public byte[] File { get; set; }
            public string MimeType { get; set; }
            public bool FileCompressed { get; set; }
        }
    
}
