using System;
using BeyondIT.MicroLoan.Infrastructure.Enums;

namespace BeyondIT.MicroLoan.Infrastructure.Helpers
{
    public static class FinancialUtility
    {
        public static double Pmt(double rate, double nPer, double pv, double fv = 0.0, DueDate due = DueDate.EndOfPeriod)
        {
            return PMT_Internal(rate, nPer, pv, fv, due);
        }

        private static double PMT_Internal(double rate, double nPer, double pv, double fv = 0.0, DueDate due = DueDate.EndOfPeriod)
        {
            if (nPer == 0.0)
                throw new ArgumentException();
            double num1;
            if (rate == 0.0)
            {
                num1 = (-fv - pv) / nPer;
            }
            else
            {
                double num2 = due == DueDate.EndOfPeriod ? 1.0 : 1.0 + rate;
                double num3 = Math.Pow(rate + 1.0, nPer);
                num1 = (-fv - pv * num3) / (num2 * (num3 - 1.0)) * rate;
            }
            return num1;
        }
    }
}