using BeyondIT.MicroLoan.Domain.BeyontDebtors;
using BeyondIT.MicroLoan.Domain.Role;
using BeyondIT.MicroLoan.Domain.Users;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Client
{
    public partial class Clients
    {
          public Clients()
            {
                ClientPageRoles = new HashSet<ClientPageRoles>();
                ClientUserRoles = new HashSet<ClientUserRoles>();
                Debtor = new HashSet<Debtor>();
                InverseParentClient = new HashSet<Clients>();
                Roles = new HashSet<Roles>();
            }

            public int ClientId { get; set; }
            public int? CreatedById { get; set; }
            public int? UpdatedById { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateUpdated { get; set; }
            public string Name { get; set; }
            public string SeoName { get; set; }
            public string GlCode { get; set; }
            public string PhysicalAddressCity { get; set; }
            public string PhysicalAddressPostalCode { get; set; }
            public string PostalAddress { get; set; }
            public bool IsSystemClient { get; set; }
            public int? ParentClientId { get; set; }
            public bool Active { get; set; }

            public virtual User CreatedBy { get; set; }
            public virtual Clients ParentClient { get; set; }
            public virtual User UpdatedBy { get; set; }
            public virtual ICollection<ClientPageRoles> ClientPageRoles { get; set; }
            public virtual ICollection<ClientUserRoles> ClientUserRoles { get; set; }
            public virtual ICollection<Debtor> Debtor { get; set; }
            public virtual ICollection<Clients> InverseParentClient { get; set; }
            public virtual ICollection<Roles> Roles { get; set; }
        }
    }
