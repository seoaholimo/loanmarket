using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Loans
{
    public partial class LoanSettings
    {
        public LoanSettings()
        {
            Loan = new Loan();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public decimal? Rate { get; set; }
        public short? IsActive { get; set; }
        public DateTime? DateRecorded { get; set; }

        public virtual Loan Loan { get; set; }
    }
}
