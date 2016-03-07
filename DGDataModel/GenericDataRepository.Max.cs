#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DG.Data.Model
{
    public partial class GenericDataRepository<T, M> : IGenericDataRepository<T, M>
        where T : class
        where M : class
    {
        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object Max(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> selector)
        {
            object ret = null;
            if (selector == null)
                return ret;

            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);

                Expression body = selector.Body;
                if (body.NodeType == ExpressionType.Convert)
                    body = ((UnaryExpression)body).Operand;
                if (body.Type == typeof(decimal))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, decimal>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(Nullable<decimal>))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, Nullable<decimal>>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(double))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, double>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(Nullable<double>))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, Nullable<double>>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(float))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, float>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(Nullable<float>))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, Nullable<float>>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(int))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, int>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(Nullable<int>))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, Nullable<int>>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(long))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, long>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
                else if (body.Type == typeof(Nullable<long>))
                    ret = dbQuery.Max(Expression.Lambda<Func<T, Nullable<long>>>(Expression.Convert(selector.Body, body.Type), selector.Parameters));
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object Max(Expression<Func<T, object>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public decimal Max(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            decimal ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public decimal Max(Expression<Func<T, decimal>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<decimal> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<decimal>>> selector)
        {
            Nullable<decimal> ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<decimal> Max(Expression<Func<T, Nullable<decimal>>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public double Max(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector)
        {
            double ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public double Max(Expression<Func<T, double>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<double> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<double>>> selector)
        {
            Nullable<double> ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<double> Max(Expression<Func<T, Nullable<double>>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public float Max(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            float ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public float Max(Expression<Func<T, float>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<float> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<float>>> selector)
        {
            Nullable<float> ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<float> Max(Expression<Func<T, Nullable<float>>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public int Max(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            int ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public int Max(Expression<Func<T, int>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<int> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<int>>> selector)
        {
            Nullable<int> ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<int> Max(Expression<Func<T, Nullable<int>>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public long Max(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            long ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public long Max(Expression<Func<T, long>> selector)
        {
            return Max(null, selector);
        }

        /// <summary>
        /// Get the Max value from items using a predicate and a selector
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<long> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<long>>> selector)
        {
            Nullable<long> ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                if (predicate != null)
                    dbQuery = dbQuery.Where(predicate);
                ret = dbQuery.Max(selector);
            }
            return ret;
        }

        /// <summary>
        /// Get the Max value from all items using a selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Nullable<long> Max(Expression<Func<T, Nullable<long>>> selector)
        {
            return Max(null, selector);
        }
    }
}
