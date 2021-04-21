#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Collections.Generic;
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
        /// Get items using a predicate, order items and limit with an offset the selection, load related object using navigationProperties
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        public virtual IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, Nullable<int> skip, Nullable<int> take, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> ret = new List<T>();
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

                    //limits
                    if (skip == null && take == null)
                        ret = dbQueryOrdered.ToList<T>();
                    else if (skip != null && take != null)
                        ret = dbQueryOrdered.Skip((int)skip).Take((int)take).ToList<T>();
                    else if (skip != null && take == null)
                        ret = dbQueryOrdered.Skip((int)skip).ToList<T>();
                    else if (skip == null && take != null)
                        ret = dbQueryOrdered.Take((int)take).ToList<T>();
                }
                else
                {
                    //limits
                    if (take != null)
                        ret = dbQuery.Take((int)take).ToList<T>();
                    else
                        ret = dbQuery.ToList<T>();
                }
            }
            return ret;
        }

        /// <summary>
        /// Get items using a predicate, order items and limit with an offset the selection
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        public IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, Nullable<int> skip, Nullable<int> take)
        {
            return List(predicate, orderby, skip, take, new Expression<Func<T, object>>[] { });
        }

        /// <summary>
        /// Get items using a predicate, order items and limit the selection
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, Nullable<int> take)
        {
            return List(predicate, orderby, null, take);
        }

        /// <summary>
        /// Get items using a predicate, order items
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby)
        {
            return List(predicate, orderby, null, null);
        }

        /// <summary>
        /// Get all items, order items and limit with an offset the selection
        /// </summary>
        /// <param name="orderby"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IList<T> List(IGenericDataOrder<T> orderby, Nullable<int> skip, Nullable<int> take)
        {
            return List(null, orderby, skip, take);
        }

        /// <summary>
        /// Get all items, order items and limit the selection
        /// </summary>
        /// <param name="orderby"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IList<T> List(IGenericDataOrder<T> orderby, Nullable<int> take)
        {
            return List(null, orderby, null, take);
        }

        /// <summary>
        /// Get all items, order items
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<T> List(IGenericDataOrder<T> orderby)
        {
            return List(null, orderby, null, null);
        }

        /// <summary>
        /// Get items using a predicate, limit the selection
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IList<T> List(Expression<Func<T, bool>> predicate, Nullable<int> take)
        {
            return List(predicate, null, null, take);
        }

        /// <summary>
        /// Get items using a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<T> List(Expression<Func<T, bool>> predicate)
        {
            return List(predicate, null, null, null);
        }

        /// <summary>
        /// Get all items 
        /// </summary>
        /// <returns></returns>
        public IList<T> List()
        {
            return List(null, null, null, null);
        }
    }
}
