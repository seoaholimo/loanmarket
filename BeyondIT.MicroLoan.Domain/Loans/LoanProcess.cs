using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Loans
{
    public partial class LoanProcess
    {
        public int Id { get; set; }
        public int? StageId { get; set; }
        public int? LoanId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateRecorded { get; set; }

        public virtual Loan Loan { get; set; }
        public virtual LoanStage Stage { get; set; }
    }
}
