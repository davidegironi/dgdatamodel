﻿#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using System.Linq;
using System.Linq.Expressions;

namespace DG.Data.Model
{
    public partial class GenericDataRepository<T, M> : IGenericDataRepository<T, M>
        where T : class
        where M : class
    {
        /// <summary>
        /// Get the first or default item using a predicate, order items, load related object using navigationProperties
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, params Expression<Func<T, object>>[] navigationProperties)
        {
            T ret = null;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                IQueryable<T> dbQuery = context.Set<T>();
                //apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                //set the predicate
                if (predicate != null)
                    dbQuery = dbQuery.AsNoTracking().Where(predicate);

                if (orderby != null)
                {
                    //apply order
                    IOrderedQueryable<T> dbQueryOrdered = orderby.ApplyOrders(dbQuery);

                    ret = dbQueryOrdered.FirstOrDefault();
                }
                else
                {
                    ret = dbQuery.FirstOrDefault();
                }
            }
            return ret;
        }

        /// <summary>
        /// Get the first or default item using a predicate, order items
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby)
        {
            return FirstOrDefault(predicate, orderby, new Expression<Func<T, object>>[] { });
        }

        /// <summary>
        /// Get the first or default item using a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return FirstOrDefault(predicate, null);
        }

        /// <summary>
        /// Get the first or default item, order items
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public T FirstOrDefault(IGenericDataOrder<T> orderby)
        {
            return FirstOrDefault(null, orderby);
        }

        /// <summary>
        /// Get the first or default item
        /// </summary>
        /// <returns></returns>
        public T FirstOrDefault()
        {
            return FirstOrDefault(null, null);
        }
    }
}
