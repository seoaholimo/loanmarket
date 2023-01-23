using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace BeyondIT.MicroLoan.Infrastructure.EnumHelpers
{
    public class FriendlyEnumType<T> where T:struct 
    {
        public T EnumType { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }

        public static List<FriendlyEnumType<T>> CreateFriendlyEnumList()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new FriendlyEnumType<T>
            {
                Id = GetEnumValue(e),
                Description = e.ToString().Humanize(LetterCasing.Title),
                EnumType = e
            }).ToList();
        }

        private static int GetEnumValue(T e)
        {
            return (int)Convert.ChangeType(e, typeof(int));
        }

        public static FriendlyEnumType<T> CreateFriendlyEnumType(string enumDescription)
        {
            var enumType = (T) Enum.Parse(typeof(T), enumDescription);
            return new FriendlyEnumType<T>
            {
                Id = GetEnumValue(enumType),
                Description = enumDescription.Humanize(LetterCasing.Title),
                EnumType = enumType
            };
        }
    }
}
