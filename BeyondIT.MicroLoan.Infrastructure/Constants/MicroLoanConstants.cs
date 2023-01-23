using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BeyondIT.MicroLoan.Infrastructure.Constants
{
    public static class MicroLoanConstants
    {
         static MicroLoanConstants()
        {
            var gbCultureInfo = new CultureInfo("en-GB");
            DateTimeFormatInfo dateTimeFormatInfo = gbCultureInfo.DateTimeFormat;
            dateTimeFormatInfo.LongDatePattern = "dd MMMM yyyy";
            ZaCultureInfo = new CultureInfo("en-ZA")
            {
                DateTimeFormat = dateTimeFormatInfo,
                NumberFormat = { NumberDecimalSeparator = "." }
            };
        }
        public const string DefaultClient = "BeyondIT";
        public const string IndividualClient = "individual";
        public const string DefaultBrowserClient = "default";
        public const string TemplateClient = "templates";
        public const string ApplicationClient = "application";
        public const string NonSeoCharRegexPattern = "[^a-zA-Z0-9-]";
        public const int SuperUserRoleId = -1;
        public const int PercentagePrecision = 5;
        public const int MoneyPrecision = 18;
        public const double PercentageMin = -999.99d;
        public const double PercentageMax = 999.99d;
        public static readonly IFormatProvider ZaFormatProvider = new CultureInfo("en-ZA");
        public static CultureInfo ZaCultureInfo { get; }
        public const string BeyondITAuthenticationScheme = "BeyondITAuthenticationScheme";
        public const string ParentNavigationPageKey = "ParentNavigationPage";
        public const string NavigationsKey = "NavigationItems";
        public const string ApiArea = "Api";
        public const string TemplatesArea = "Templates";

        public const string ChevronSeperator =
            "<i class=\"fa fa-chevron-right mf-chevron-separator\" aria-hidden=\"true\"></i>";

    }
}
