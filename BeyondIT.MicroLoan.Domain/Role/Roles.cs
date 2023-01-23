using BeyondIT.MicroLoan.Domain.Client;
using BeyondIT.MicroLoan.Domain.Users;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Role
{
    public partial class Roles
    {
        public Roles()
        {
            ClientPageRoles = new HashSet<ClientPageRoles>();
            ClientUserRoles = new HashSet<ClientUserRoles>();
            InverseParentRole = new HashSet<Roles>();
        }

        public int RoleId { get; set; }
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public bool IsAdminRole { get; set; }
        public int? ParentRoleId { get; set; }

        public virtual Clients Client { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual Roles ParentRole { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual ICollection<ClientPageRoles> ClientPageRoles { get; set; }
        public virtual ICollection<ClientUserRoles> ClientUserRoles { get; set; }
        public virtual ICollection<Roles> InverseParentRole { get; set; }

    }
}
