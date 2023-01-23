using System;
using System.Collections.Generic;
using System.Linq;

namespace BeyondIT.MicroLoan.Infrastructure.Types
{
    public class PagedResults<T>
    {
        public PagedResults(IEnumerable<T> enumerable, PageInfo pageInfo)
        {
            int totalItems = enumerable.Count();
            if (pageInfo == null)
            {
                pageInfo = new PageInfo(PageInfo.DefaultStartPage, totalItems);
            }

            pageInfo.TotalItems = totalItems;
            Items = enumerable.Skip(pageInfo.PageSize * (pageInfo.Page - 1)).Take(pageInfo.PageSize).ToList();
            PageInfo = pageInfo;
            PageInfo.TotalItems = totalItems;
        }

        private PagedResults()
        {
        }

        public List<T> Items { get; private set; }
        public PageInfo PageInfo { get; private set; }

        public PagedResults<TMapped> MapToType<TMapped>(Func<List<T>, List<TMapped>> func)
        {
            return new PagedResults<TMapped>
            {
                Items = func.Invoke(Items),
                PageInfo = PageInfo
            };
        }
    }
}