using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Loans
{
    public partial class LoanStage
    {
        public LoanStage()
        {
            LoanProcess = new HashSet<LoanProcess>();
        }

        public int Id { get; set; }
        public string StageStatus { get; set; }
        public string StageDescription { get; set; }
        public int? RoleId { get; set; }
        public short? IsActive { get; set; }
        public DateTime? DateRecorded { get; set; }

        public virtual ICollection<LoanProcess> LoanProcess { get; set; }
    }
}
