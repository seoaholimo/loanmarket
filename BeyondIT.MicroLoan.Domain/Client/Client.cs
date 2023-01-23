using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Domain.Contacts;
using BeyondIT.MicroLoan.Domain.Role;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Client
{
    public class Client : BaseEntity, IActiveFlag
    {
        public Client()
        {
            PhysicalAddress = new Address();
            PostalAddress = new Address();
            Active = true;
        }

        public int ClientId { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public string GlCode { get; set; }
        public virtual Address PhysicalAddress { get; set; }
        public virtual Address PostalAddress { get; set; }
        public virtual ClientDetail ClientDetails { get; set; }
        public bool IsSystemClient { get; set; }
        public int? ParentClientId { get; set; }
        public virtual Client ParentClient { get; set; }
        public virtual ICollection<Client> SubClients { get; set; }
        public virtual ICollection<ClientLevel> ClientLevels { get; set; }
        public int? ClientLevelId { get; set; }
        public virtual ClientLevel ClientLevel { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public bool Active { get; set; }
    
    }
}