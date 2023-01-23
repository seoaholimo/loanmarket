using BeyondIT.MicroLoan.Domain.Page;
using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Domain.Navigation
{
    public partial class Navigations
    {
        public Navigations()
        {
            InverseParentNavigation = new HashSet<Navigations>();
        }

        public int NavigationId { get; set; }
        public string Title { get; set; }
        public int? PageId { get; set; }
        public int? ParentNavigationId { get; set; }
        public bool ShowItem { get; set; }
        public bool OrderedChildItems { get; set; }
        public int GroupOrder { get; set; }
        public string QueryString { get; set; }

        public virtual Pages Page { get; set; }
        public virtual Navigations ParentNavigation { get; set; }
        public virtual ICollection<Navigations> InverseParentNavigation { get; set; }
    }
}
