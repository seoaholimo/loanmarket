using System;
using System.Collections.Generic;

namespace BeyondIT.MicroLoan.Infrastructure.Csv
{
    public class CsvField
    {
        public CsvField(string fieldName, Func<object, object> formatValue = null)
        {
            FieldName = fieldName;
            FormatValue = formatValue;
            ValidValues = new List<string>();
        }

        public string FieldName { get; }
        public Func<object, object> FormatValue { get; }
        public bool Required { get; set; }
        public List<string> ValidValues { get; set; }
    }
}