using System.Collections.Generic;
using System.Linq;

namespace BeyondIT.MicroLoan.Infrastructure
{
    
    public class PageInfo
    {
        public const int DefaultStartPage = 1;
        public const int DefaultPageSize = 15;

        public PageInfo(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; }
        public int PageSize { get; }
        public int TotalItems { get; set; }

        public static PageInfo CreateDefaultPageInfo()
        {
            return new PageInfo(DefaultStartPage, DefaultPageSize);
        }
    }
}
