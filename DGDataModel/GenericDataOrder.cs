#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Linq;
using System.Linq.Expressions;

namespace DG.Data.Model
{
    public class GenericDataOrder<T, TKey> : IGenericDataOrder<T>
        where T : class
    {
        /// <summary>
        /// Sorting direction
        /// </summary>
        public bool SortAscending { get; set; }

        /// <summary>
        /// Entity selector
        /// </summary>
        public Expression<Func<T, TKey>> Selector { get; private set; }

        /// <summary>
        /// Caller
        /// </summary>
        private object _caller = null;

        /// <summary>
        /// Order constructor
        /// </summary>
        /// <param name="caller"></param>
        public GenericDataOrder(object caller)
        {
            _caller = caller;
            SortAscending = true;
            Selector = null;
        }

        /// <summary>
        /// Order constructor that sets the direction and the selector
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="selector"></param>
        /// <param name="sortAscending"></param>
        public GenericDataOrder(object caller, Expression<Func<T, TKey>> selector, bool sortAscending)
        {
            _caller = caller;
            SortAscending = sortAscending;
            Selector = selector;
        }

        /// <summary>
        /// Get the parent IGenericDataOrder
        /// </summary>
        public IGenericDataOrder<T> GetParent()
        {
            IGenericDataOrder<T> parent = _caller as IGenericDataOrder<T>;
            if (parent != null)
                return parent;
            else
                return null;
        }

        /// <summary>
        /// Ascending order
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IGenericDataOrder<T> ThenBy<TKey1>(Expression<Func<T, TKey1>> selector)
        {
            return new GenericDataOrder<T, TKey1>(this, selector, true);
        }

        /// <summary>
        /// Descending order
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IGenericDataOrder<T> ThenByDescending<TKey1>(Expression<Func<T, TKey1>> selector)
        {
            return new GenericDataOrder<T, TKey1>(this, selector, false);
        }

        /// <summary>
        /// Apply order to query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> ApplyOrders(IQueryable<T> query)
        {
            //get the order list
            IGenericDataOrder<T>[] orderlist = new IGenericDataOrder<T>[] { };
            IGenericDataOrder<T> currentOrder = this;
            while (currentOrder != null)
            {
                orderlist = orderlist.Concat(new IGenericDataOrder<T>[] { currentOrder }).ToArray();
                IGenericDataOrder<T> parentOrder = currentOrder.GetParent();
                if (parentOrder != null)
                    currentOrder = parentOrder;
                else
                    currentOrder = null;
            }
            orderlist = orderlist.Reverse().ToArray();

            //build the ordered query
            IOrderedQueryable<T> queryOrdered = null;
            foreach (IGenericDataOrder<T> order in orderlist)
            {
                if (queryOrdered == null)
                {
                    if (order.SortAscending)
                        queryOrdered = order.ApplyOrderByAscending(query);
                    else
                        queryOrdered = order.ApplyOrderByDescending(query);
                }
                else
                {
                    if (order.SortAscending)
                        queryOrdered = order.ApplyThenByAscending(queryOrdered);
                    else
                        queryOrdered = order.ApplyThenByDescending(queryOrdered);
                }
            }

            return queryOrdered;
        }

        /// <summary>
        /// Apply order to query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> ApplyOrderByAscending(IQueryable<T> query)
        {
            return query.OrderBy(Selector);
        }

        /// <summary>
        /// Apply order to query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> ApplyOrderByDescending(IQueryable<T> query)
        {
            return query.OrderByDescending(Selector);
        }

        /// <summary>
        /// Apply order to ordered query
        /// </summary>
        /// <param name="queryOrdered"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> ApplyThenByAscending(IOrderedQueryable<T> queryOrdered)
        {
            return queryOrdered.ThenBy(Selector);
        }

        /// <summary>
        /// Apply order to ordered query
        /// </summary>
        /// <param name="queryOrdered"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> ApplyThenByDescending(IOrderedQueryable<T> queryOrdered)
        {
            return queryOrdered.ThenByDescending(Selector);
        }
    }
}

