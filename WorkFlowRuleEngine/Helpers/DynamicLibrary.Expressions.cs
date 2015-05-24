using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Linq.Dynamic
{
    public static partial class DynamicQueryable
    {
        public static Expression Parse(Type resultType, string expression, params object[] values)
        {
            ExpressionParser parser = new ExpressionParser(null, expression, values);
            return parser.Parse(resultType);
        }

        public static LambdaExpression ParseLambda(Type itType, Type resultType, string expression, params object[] values)
        {
            return ParseLambda(new ParameterExpression[] { Expression.Parameter(itType, "") }, resultType, expression, values);
        }

        public static LambdaExpression ParseLambda(ParameterExpression[] parameters, Type resultType, string expression, params object[] values)
        {
            ExpressionParser parser = new ExpressionParser(parameters, expression, values);
            return Expression.Lambda(parser.Parse(resultType), parameters);
        }

        public static Expression<Func<T, S>> ParseLambda<T, S>(string expression, params object[] values)
        {
            return (Expression<Func<T, S>>)ParseLambda(typeof(T), typeof(S), expression, values);
        }

        public static Expression<Func<T, S>> ParseLambda<T, S>(string expression)
        {

            //Func<string, string> func = DynamicExpression.ParseLambda<string, string>("f => f.Substring(0, f.IndexOf(\" \"))").Compile();

            string paramString = expression.Substring(0, expression.IndexOf("=>")).Trim();
            string lambdaString = expression.Substring(expression.IndexOf("=>") + 2).Trim();
            ParameterExpression param = Expression.Parameter(typeof(T), paramString);
            return (Expression<Func<T, S>>)ParseLambda(new[] { param }, typeof(S), lambdaString, null);
        }

        public static LambdaExpression ParseLambda(string expression, Type returnType, params Type[] paramTypes)
        {
            //Func<string, int, string> otherFunc = ((Expression<Func<string, int, string>>)DynamicExpression.ParseLambda("(str, ind) => (ind * 100).ToString() + str")).Compile();
            string paramString = expression.Substring(0, expression.IndexOf("=>")).Trim("() ".ToCharArray());
            string lambdaString = expression.Substring(expression.IndexOf("=>") + 2).Trim();
            var paramList = paramString.Split(',');
            if (paramList.Length != paramTypes.Length)
                throw new ArgumentException("Specified number of lambda parameters do not match the number of parameter types!", "expression");

            List<ParameterExpression> parameters = new List<ParameterExpression>();
            for (int i = 0; i < paramList.Length; i++)
                parameters.Add(Expression.Parameter(paramTypes[i], paramList[i]));

            return ParseLambda(parameters.ToArray(), returnType, lambdaString, null);
        }


        //public static Expression<Func<T, bool>> strToFunc<T>(string propName, string opr, string value, Expression<Func<T, bool>> expr = null)
        //{
        //    Expression<Func<T, bool>> func = null;
        //    try
        //    {
        //        //Expression.
        //        var prop = GetProperty<T>(propName);
        //        ParameterExpression tpe = Expression.Parameter(typeof(T));
        //        Expression left = Expression.Property(tpe, prop);
        //        Expression right = Expression.Convert(ToExprConstant(prop, value), prop.PropertyType);
        //        Expression<Func<T, bool>> innerExpr = Expression.Lambda<Func<T, bool>>(ApplyFilter(opr, left, right), tpe);
        //        if (expr != null)
        //            innerExpr = innerExpr.And(expr);
        //        func = innerExpr;
        //    }
        //    catch { }
        //    return func;
        //}
       
        //private static BinaryExpression ApplyFilter(string opr, Expression left, Expression right)
        //{
        //    BinaryExpression InnerLambda = null;
        //    switch (opr)
        //    {
        //        case "==":
        //        case "=":
        //            InnerLambda = Expression.Equal(left, right);
        //            break;
        //        case "<":
        //            InnerLambda = Expression.LessThan(left, right);
        //            break;
        //        case ">":
        //            InnerLambda = Expression.GreaterThan(left, right);
        //            break;
        //        case ">=":
        //            InnerLambda = Expression.GreaterThanOrEqual(left, right);
        //            break;
        //        case "<=":
        //            InnerLambda = Expression.LessThanOrEqual(left, right);
        //            break;
        //        case "!=":
        //            InnerLambda = Expression.NotEqual(left, right);
        //            break;
        //        case "&&":
        //            InnerLambda = Expression.And(left, right);
        //            break;
        //        case "||":
        //            InnerLambda = Expression.NotEqual(left, right);
        //            break;
        //    }
        //    return InnerLambda;
        //}
        //public static Expression<Func<T, TResult>> And<T, TResult>(this Expression<Func<T, TResult>> expr1, Expression<Func<T, TResult>> expr2)
        //{
        //    var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        //    return Expression.Lambda<Func<T, TResult>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        //}
        //public static Func<T, TResult> ExpressionToFunc<T, TResult>(this Expression<Func<T, TResult>> expr)
        //{
        //    return expr.Compile();
        //}
    }
}
