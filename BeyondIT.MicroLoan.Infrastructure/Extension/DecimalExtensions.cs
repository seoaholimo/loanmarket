using System;
using BeyondIT.MicroLoan.Infrastructure.Constants;

namespace BeyondIT.MicroLoan.Infrastructure.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToMoneyString(this decimal amount)
        {
            return amount.ToString("C", MicroLoanConstants.ZaFormatProvider);
        }

        public static string ToMoneyString(this decimal? amount)
        {
            return amount.HasValue ? ToMoneyString(amount.Value) : string.Empty;
        }

        public static string ToMoneyString(this double amount)
        {
            return amount.ToString("C", MicroLoanConstants.ZaFormatProvider);
        }

        public static decimal ToTwoDecimalPlaces(this decimal amount)
        {
            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }

        public static double ToTwoDecimalPlaces(this double amount)
        {
            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }
    }
}