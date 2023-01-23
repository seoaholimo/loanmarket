using BeyondIT.MicroLoan.Domain.Client;
using BeyondIT.MicroLoan.Domain.Page;
using BeyondIT.MicroLoan.Domain.Users;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Role
{
    public partial class ClientPageRoles
    {
        public int ClientId { get; set; }
        public int PageId { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual Clients Client { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual Pages Page { get; set; }
        public virtual Roles Role { get; set; }
        public virtual User UpdatedBy { get; set; }
    }
}
