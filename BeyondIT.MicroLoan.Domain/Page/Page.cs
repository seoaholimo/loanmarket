using BeyondIT.MicroLoan.Domain.Attributes;

namespace BeyondIT.MicroLoan.Domain.Page
{
    [TsClass]
    public class Page
    {
        public int PageId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool ValidateUserRole { get; set; }
        public bool SuperUserOnly { get; set; }
        public PageKey RouteKey { get; set; }
    }
}
