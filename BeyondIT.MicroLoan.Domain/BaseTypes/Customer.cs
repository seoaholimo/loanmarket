using System;

namespace BeyondIT.MicroLoan.Domain.BaseTypes
{
    public class Customer 
    {
        public int CustomerId { get; set; }
        public DateTime CustomerCreatedDate { get; set; }
        public string IdentityDocumentNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string CellphoneNumber { get; set; }
        public string TelephoneNumber { get; set; }
    }
}