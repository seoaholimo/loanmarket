using System;

namespace BeyondIT.MicroLoan.Infrastructure.EnumHelpers
{
    public static class EnumExtensions
    {
        public static int Value(this Enum e)
        {
            return (int)Convert.ChangeType(e, typeof(int));
        }
    }
}