using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Domain.Client;
using BeyondIT.MicroLoan.Domain.Loans;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.BeyontDebtors
{
    public partial class Debtor
    {
        public Debtor()
        {
            LoanLedger = new HashSet<LoanLedger>();
        }

        public int DebtorId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string IdentificationNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public int? AddressId { get; set; }
        public DateTime? DateRecorded { get; set; }
        public short? IsActive { get; set; }
        public int? ClientId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Clients Client { get; set; }
        public virtual ICollection<LoanLedger> LoanLedger { get; set; }
    }
}
