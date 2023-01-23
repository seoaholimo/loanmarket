using BeyondIT.MicroLoan.Domain.Attributes;
using BeyondIT.MicroLoan.Domain.BeyontDebtors;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.BaseTypes
{
    [TsClass]
    public partial class Address
    {
            public Address()
            {
                Debtor = new HashSet<Debtor>();
            }

            public int Id { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string TelNo { get; set; }
            public string CellNo { get; set; }
            public string District { get; set; }
            public string Village { get; set; }
            public DateTime? DateRecorded { get; set; }
            public short? IsActive { get; set; }

            public virtual ICollection<Debtor> Debtor { get; set; }
        
    }
}
