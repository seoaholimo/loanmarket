using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Loans
{
    public partial class Loan
    {
        public Loan()
        {
            LoanLedger = new HashSet<LoanLedger>();
            LoanProcess = new HashSet<LoanProcess>();
            Setting = new HashSet<LoanSettings>();
        }

        public int LoanId { get; set; }
        public decimal? AmountRequest { get; set; }
        public decimal? AmountApproved { get; set; }
        public string LoanStatus { get; set; }
        public int? DebtorId { get; set; }
        public short? IsActive { get; set; }
        public string Description { get; set; }
        public DateTime? DateRecorded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? PaymentFrequency { get; set; }
        public int? PaymentAmount { get; set; }
        public int? StageId { get; set; }

        public virtual ICollection<LoanLedger> LoanLedger { get; set; }
        public virtual ICollection<LoanProcess> LoanProcess { get; set; }
        public virtual ICollection<LoanSettings> Setting{ get; set; }
    }
}
