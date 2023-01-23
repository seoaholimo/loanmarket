using System;
using BeyondIT.MicroLoan.Domain.Attributes;

namespace BeyondIT.MicroLoan.Domain.Contacts.Commands
{
    [TsClass]
    public class ContactCommand
    {
        public virtual int ContactId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Position { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string TelephoneNumber { get; set; }
        public virtual string CellphoneNumber { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual string Hobbies { get; set; }
    }
}