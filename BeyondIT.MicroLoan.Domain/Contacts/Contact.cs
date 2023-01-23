using System;
using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Domain.Client;

namespace BeyondIT.MicroLoan.Domain.Contacts
{
    public class Contact : BaseEntity
    {
        public int ContactId { get; set; }
        public int ClientId { get; set; }
        public virtual Clients Client { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string CellphoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string FullName => $"{Name} {Surname}";
    }
}
