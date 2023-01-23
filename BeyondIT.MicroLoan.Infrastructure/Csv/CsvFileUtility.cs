using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BeyondIT.MicroLoan.Domain.Exceptions;
using BeyondIT.MicroLoan.Infrastructure.Csv;
using CsvHelper;


namespace MBeyondIT.MicroLoan.Infrastructure.Csv
{
    public class CsvFileUtility
    {
        private readonly Dictionary<string, int> _fieldsIndex = new Dictionary<string, int>();

        public List<TModel> GetItemsFromCsvFileStream<TModel, TProperty>(Stream csvFileStream,
            Dictionary<Expression<Func<TModel, TProperty>>, CsvField> objectCsvFieldsMap) where TModel : new()
        {
            List<TModel> models = new List<TModel>();

            TextReader textReader = new StreamReader(csvFileStream);
            var csv = new CsvReader(textReader);
            csv.Read();
            csv.ReadHeader();

            var fields = new List<string>();
            int fieldIndex = 0;
            while (csv.TryGetField(fieldIndex, out string field))
            {
                fields.Add(field);
                fieldIndex++;
            }

            //Validate fields
            List<CsvField> requiredFields = objectCsvFieldsMap.Values.Where(f => f.Required).ToList();
            ValidateFields(requiredFields, fields);

            //Columns
            int index = 0;
            foreach (string field in fields)
            {
                _fieldsIndex.Add(field, index);
                index++;
            }

            while (csv.Read())
            {
                CsvField csvField = null;
                try
                {
                    //Create object
                    TModel model = new TModel();

                    foreach (Expression<Func<TModel, TProperty>> expression in objectCsvFieldsMap.Keys)
                    {
                        MemberExpression member = expression.Body as MemberExpression;

                        if (member == null)
                        {
                            UnaryExpression ubody = (UnaryExpression) expression.Body;
                            member = (MemberExpression) ubody.Operand;
                        }

                        PropertyInfo propInfo = (PropertyInfo) member.Member;

                        csvField = objectCsvFieldsMap[expression];

                        if (!_fieldsIndex.ContainsKey(csvField.FieldName)) continue;

                        ValidateFieldValues(csvField, csv.GetField(_fieldsIndex[csvField.FieldName]));

                        if (propInfo.PropertyType == typeof(string))
                        {
                            string value = csv.GetField(_fieldsIndex[csvField.FieldName]);

                            value = csvField.FormatValue?.Invoke(value).ToString() ?? value;

                            propInfo.SetValue(model, value, null);
                        }
                        else
                        {
                            if (propInfo.PropertyType == typeof(int) ||
                                propInfo.PropertyType == typeof(int?))
                            {
                                var value = GetNullableFieldValue<int>(csvField,
                                    csv.GetField(_fieldsIndex[csvField.FieldName]));
                                propInfo.SetValue(model, value, null);
                            }

                            if (propInfo.PropertyType == typeof(long) ||
                                propInfo.PropertyType == typeof(long?))
                            {
                                var value = GetNullableFieldValue<long>(csvField,
                                    csv.GetField(_fieldsIndex[csvField.FieldName]));
                                propInfo.SetValue(model, value, null);
                            }

                            if (propInfo.PropertyType == typeof(double) ||
                                propInfo.PropertyType == typeof(double?))
                            {
                                var value = GetNullableFieldValue<double>(csvField,
                                    csv.GetField(_fieldsIndex[csvField.FieldName]));
                                propInfo.SetValue(model, value, null);
                            }

                            if (propInfo.PropertyType == typeof(decimal) ||
                                propInfo.PropertyType == typeof(decimal?))
                            {
                                var value = GetNullableFieldValue<decimal>(csvField,
                                    csv.GetField(_fieldsIndex[csvField.FieldName]));
                                propInfo.SetValue(model, value, null);
                            }

                            if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?))
                            {
                                var value = GetNullableFieldValue<DateTime>(csvField,
                                    csv.GetField(_fieldsIndex[csvField.FieldName]));
                                propInfo.SetValue(model, value, null);
                            }
                        }
                    }

                    models.Add(model);
                }
                catch (Exception exception)
                {
                    string value = csv.GetField(_fieldsIndex[csvField.FieldName]);
                    string errorMessage =
                        $"{exception.Message}. Field \"{csvField.FieldName}\", value \"{value}\"";
                    throw new BeyondITLoanException(errorMessage);
                }
            }

            return models;
        }

        public static void ValidateFields(IEnumerable<CsvField> validCsvFields, IEnumerable<string> uploadedFileFields)
        {
            var fieldNames = validCsvFields.Select(f => f.FieldName).ToList();
            string notFoundField = fieldNames.FirstOrDefault(field =>
                !uploadedFileFields.Contains(field, StringComparer.CurrentCultureIgnoreCase));
            if (!string.IsNullOrEmpty(notFoundField))
            {
                throw new BeyondITLoanException($"The field \"{notFoundField}\" could not be found from uploaded file.");
            }
        }

        public static void ValidateFieldValues(CsvField csvField, string value)
        {
            if (csvField.ValidValues.Any())
            {
                if (!csvField.ValidValues.Contains(value))
                {
                    throw new BeyondITLoanException(
                        $"{value} is not valid value for {csvField.FieldName}");
                }
            }
        }

        public T? GetNullableFieldValue<T>(CsvField csvField, string inputValue) where T : struct
        {
            var value = !string.IsNullOrEmpty(inputValue)
                ? (T) Convert.ChangeType(inputValue.Trim(), typeof(T),
                    new CultureInfo("en-GB"))
                : new T?();

            if (value != null && csvField.FormatValue != null)
            {
                value = (T) Convert.ChangeType(csvField.FormatValue(value), typeof(T));
            }

            return value;
        }

        public T GetFieldValue<T>(CsvField csvField, string inputValue) where T : struct
        {
            var fieldValue = GetNullableFieldValue<T>(csvField, inputValue);

            return fieldValue ?? default(T);
        }
    }
}