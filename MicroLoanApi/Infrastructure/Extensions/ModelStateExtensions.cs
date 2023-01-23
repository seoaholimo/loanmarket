using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            List<string> errors = new List<string>();
            foreach (ModelStateEntry modelStateEntry in modelState.Values)
            {
                foreach (ModelError error in modelStateEntry.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }

        public static void ValidationBlackListFor<TModel>(this ModelStateDictionary modelState, IEnumerable<Expression<Func<TModel, object>>> expressions)
        {
            foreach (Expression<Func<TModel, object>> expression in expressions)
            {
                string expressionText = ExpressionHelper.GetExpressionText(expression);

                List<KeyValuePair<string, ModelStateEntry>> states = modelState.ToList();

                foreach (KeyValuePair<string, ModelStateEntry> ms in states)
                {
                    if (ms.Key.StartsWith(expressionText + ".") || ms.Key == expressionText)
                    {
                        modelState.Remove(ms.Key);
                    }
                }
            }
        }

        public static void ValidationWhiteListFor<TModel>(this ModelStateDictionary modelState, IEnumerable<Expression<Func<TModel, object>>> expressions)
        {
            List<string> expressionTexts = expressions.Select(ExpressionHelper.GetExpressionText).ToList();

            List<KeyValuePair<string, ModelStateEntry>> modelStateEntries = modelState.ToList();

            foreach (KeyValuePair<string, ModelStateEntry> modelStateEntry in modelStateEntries)
            {
                if (!expressionTexts.Contains(modelStateEntry.Key) || string.IsNullOrEmpty(modelStateEntry.Key))
                {
                    modelState.Remove(modelStateEntry.Key);
                }
            }
        }
    }
}