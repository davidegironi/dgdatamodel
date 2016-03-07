#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DG.Data.Model.Helpers
{
    /// <summary>
    /// Dynamic query in LINQ using a Predicate Builder
    /// Reference: http://www.c-sharpcorner.com/UploadFile/c42694/dynamic-query-in-linq-using-predicate-builder/
    /// </summary>
    public static class DGPredicateBuilder
    {
        /// <summary>
        /// Dynamically compose expression True predicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// Creates a predicate that evaluates to true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// Creates a predicate that evaluates to false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>
        /// Combines two predicates using the logical AND
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstPredicate"></param>
        /// <param name="secondPredicate"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> firstPredicate, Expression<Func<T, bool>> secondPredicate)
        {
            return firstPredicate.Compose(secondPredicate, Expression.AndAlso);
        }

        /// <summary>
        /// Combines two predicates using the logical OR
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstPredicate"></param>
        /// <param name="secondPredicate"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> firstPredicate, Expression<Func<T, bool>> secondPredicate)
        {
            return firstPredicate.Compose(secondPredicate, Expression.OrElse);
        }

        /// <summary>
        /// Negates a predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicate)
        {
            var negated = Expression.Not(predicate.Body);
            return Expression.Lambda<Func<T, bool>>(negated, predicate.Parameters);
        }

        /// <summary>
        /// Combines two predicates using the logical the specified merge function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstPredicate"></param>
        /// <param name="secondPredicate"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        private static Expression<T> Compose<T>(this Expression<T> firstPredicate, Expression<T> secondPredicate, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)  
            var map = firstPredicate.Parameters
                .Select((f, i) => new { f, s = secondPredicate.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first  
            var secondBody = ParameterRebinder.ReplaceParameters(map, secondPredicate.Body);

            // create a merged lambda expression with parameters from the first expression  
            return Expression.Lambda<T>(merge(firstPredicate.Body, secondBody), firstPredicate.Parameters);
        }

        /// <summary>
        /// Expression rewriter
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// Mapped parameters
            /// </summary>
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="map"></param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// Replace parameters
            /// </summary>
            /// <param name="map"></param>
            /// <param name="exp"></param>
            /// <returns></returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <summary>
            /// Visit each parameter
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }
}
