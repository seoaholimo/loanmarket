using System;
using System.Collections.Generic;
using System.Text;

namespace BeyondIT.MicroLoan.Application.Interfaces
{
    public class IUserContext
    {
        public int UserId { get; }
        public bool IsSuperUser { get; }
    }
}
