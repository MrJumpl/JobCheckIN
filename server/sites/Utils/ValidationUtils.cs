using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using Mlok.Core.Utils;
using System;

namespace Mlok.Web.Sites.JobChIN.Utils
{
    public static class ValidationUtils
    {
        public static IRuleBuilderInitial<T, IEnumerable<TProperty>> ListUniqueness<T, TProperty>(this IRuleBuilderInitial<T, IEnumerable<TProperty>> builder, string propertyName)
         => ListUniqueness(builder, propertyName, x => x);

        public static IRuleBuilderInitial<T, IEnumerable<TProperty>> ListUniqueness<T, TProperty, TId>(this IRuleBuilderInitial<T, IEnumerable<TProperty>> builder, string propertyName, Func<TProperty, TId> selector)
        {
            builder.Must(x => Predicate(x, selector))
                .WithMessage(_ => _.Localize($"Pole '{propertyName}' nesmí obsahovat dvě stejné hodnoty.", $"Filed '{propertyName}' must not contain two identical values."));
            return builder;
        }

        public static IRuleBuilderInitial<T, IEnumerable<TProperty>> ListAny<T, TProperty, TId>(this IRuleBuilderInitial<T, IEnumerable<TProperty>> builder, string propertyName, Func<TProperty, TId> selector)
        {
            builder.Must(x => Predicate(x, selector))
                .WithMessage(_ => _.Localize($"Pole '{propertyName}' nesmí obsahovat dvě stejné hodnoty.", $"Filed '{propertyName}' must not contain two identical values."));
            return builder;
        }

        public static IRuleBuilderInitial<T, string> ValidatePhone<T>(this IRuleBuilderInitial<T, string> builder, string propertyName)
        {
            builder.Must(PhoneNumberUtils.IsValid)
                .WithMessage(_ => _.Localize($"Pole '{propertyName}' není zadané v platném tvaru.", $"Field '{propertyName}' is not entered in a valid format."));
            return builder;
        }

        static bool Predicate<TProperty, TId>(IEnumerable<TProperty> x, Func<TProperty, TId> selector)
        {
            if (x == null)
                return true;
            var values = x.Select(selector);
            values = values.Distinct();
            var count = values.Count();
            var count2 = x.Count();
            return count == count2;
        }
    }
}