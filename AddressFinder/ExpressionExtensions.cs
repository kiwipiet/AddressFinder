using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AddressFinder
{
    public static class ExpressionExtensions
    {
        public static string PropertyName<T>(this Expression<Func<T, object>> property)
        {
            var member = property.Body as MemberExpression;
            if (member != null)
                return member.Member.Name;

            var unaryExpression = property.Body as UnaryExpression;
            if (unaryExpression != null)
                member = unaryExpression.Operand as MemberExpression;

            return member != null ? member.Member.Name : string.Empty;
        }

        public static string PropertyName<T>(this T instance, Expression<Func<T, object>> property)
        {
            return property.PropertyName();
        }
    }
}
