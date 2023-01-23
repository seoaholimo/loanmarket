using BeyondIT.MicroLoan.Domain.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BeyondIT.MicroLoan.Domain.BaseTypes
{
    [TsClass]
    public class PersonalDetails
    {
        [Required]
        public string IdentityDocumentNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string CellphoneNumber { get; set; }

        public string TelephoneNumber { get; set; }
    }
}
