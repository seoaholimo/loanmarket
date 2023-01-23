using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Extensions
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> InsertEmptyFirst(this IEnumerable<SelectListItem> list, string emptyValue, string emptyText)
        {
            return new[] { new SelectListItem { Value = emptyValue, Text = emptyText } }.Concat(list);
        }
    }
}