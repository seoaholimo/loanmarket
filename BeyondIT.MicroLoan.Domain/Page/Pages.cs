using BeyondIT.MicroLoan.Domain.Navigation;
using BeyondIT.MicroLoan.Domain.Role;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Page
{
    public partial class Pages
    {
        public Pages()
        {
            ClientPageRoles = new HashSet<ClientPageRoles>();
            Navigations = new HashSet<Navigations>();
        }

        public int PageId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool ValidateUserRole { get; set; }
        public bool SuperUserOnly { get; set; }

        public virtual ICollection<ClientPageRoles> ClientPageRoles { get; set; }
        public virtual ICollection<Navigations> Navigations { get; set; }
    }
}
