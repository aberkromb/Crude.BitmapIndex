using System;
using System.Linq.Expressions;

namespace Crude.BitmapIndex.Helpers
{
    public class PropertyNameHelper
    {
        public static string From<T, TV>(Expression<Func<T, TV>> expression) =>
            GetName(expression.Body);


        private static string GetName(Expression expression)
        {
            switch (expression)
            {
                case MemberExpression memberExpression:
                    return memberExpression.Member.Name;
                case MethodCallExpression methodCallExpression:
                    return methodCallExpression.Arguments[0].ToString();
                default:
                    throw new ArgumentException("Expression was not Property, Field or Method");
            }
        }
    }
}