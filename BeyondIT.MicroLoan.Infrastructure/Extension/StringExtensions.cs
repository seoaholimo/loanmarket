using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BeyondIT.MicroLoan.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string RegexReplace(this string inputString, string regexPattern, string replaceString)
        {
            var replaceRegex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            return replaceRegex.Replace(inputString, replaceString);
        }

        public static string ToSentenceLetterCasing(this string inputString)
        {
            string[] words = inputString.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            var newWords = new List<string>();
            foreach (string word in words)
            {
                string newWord;
                if (word.Length > 1)
                {
                    newWord = word.ToLower();
                    string firstChar = newWord.Substring(0, 1).ToUpper();
                    string restOfString = newWord.Substring(1);
                    newWord = $"{firstChar}{restOfString}";
                }
                else
                {
                    newWord = word;
                }
                newWords.Add(newWord);
            }

            return string.Join(" ", newWords);
        }

        public static void GetDatesFromSearchText(this string searchText, out DateTime fromDate, out DateTime toDate)
        {
            string[] dates = searchText.Split(new[]{"to"}, StringSplitOptions.None);
            fromDate = DateTime.Now.AddDays(-7);
            toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            if (!string.IsNullOrEmpty(dates[0]))
            {
                fromDate = Convert.ToDateTime(dates[0]);
            }

            if (!string.IsNullOrEmpty(dates[1]))
            {
                toDate = Convert.ToDateTime(dates[1]);
            }
        }
    }
}
