using System;
using System.Collections.Generic;
using System.Text;

namespace BeyondIT.MicroLoan.Domain.Exceptions
{
    public class BeyondITLoanException: Exception
    {
        public BeyondITLoanException(string message) : base(message) { }
    }
}
