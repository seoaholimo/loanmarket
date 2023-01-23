using BeyondIT.MicroLoan.Domain.BeyontDebtors;
using BeyondIT.MicroLoan.Domain.Users;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Loans
{
    public partial class LoanLedger
    {
        public int Id { get; set; }
        public int? LoanId { get; set; }
        public int? DebtorId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? DateRecorded { get; set; }
        public int? CreatedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Debtor Debtor { get; set; }
        public virtual Loan Loan { get; set; }
    }
}
