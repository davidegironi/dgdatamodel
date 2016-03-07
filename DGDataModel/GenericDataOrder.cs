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
    public class GenericDataOrder<T> : IGenericDataOrder<T>
        where T : class
    {
        /// <summary>
        /// Sorting directions
        /// </summary>
        public enum Sort { Ascending, Descending };

        /// <summary>
        /// Sorting direction
        /// </summary>
        public Sort Direction { get; private set; }

        /// <summary>
        /// Entity selector
        /// </summary>
        public Expression<Func<T, object>> Selector { get; private set; }

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
            Direction = Sort.Ascending;
            Selector = null;
        }

        /// <summary>
        /// Order constructor that sets the direction and the selector
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="selector"></param>
        /// <param name="direction"></param>
        public GenericDataOrder(object caller, Expression<Func<T, object>> selector, Sort direction)
        {
            _caller = caller;
            Direction = direction;
            Selector = selector;
        }

        /// <summary>
        /// Get the parent GenericDataOrder
        /// </summary>
        public GenericDataOrder<T> Parent
        {
            get
            {
                if (_caller.GetType() == typeof(GenericDataOrder<T>))
                    return (GenericDataOrder<T>)_caller;
                else
                    return null;
            }
        }

        /// <summary>
        /// Check if has a GenericDataOrder parent
        /// </summary>
        /// <returns></returns>
        public bool HasParent()
        {
            if (_caller.GetType() != typeof(GenericDataOrder<T>))
                return false;
            return true;
        }

        /// <summary>
        /// Ascending order
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IGenericDataOrder<T> ThenBy(Expression<Func<T, object>> selector)
        {
            return new GenericDataOrder<T>(this, selector, GenericDataOrder<T>.Sort.Ascending);
        }

        /// <summary>
        /// Descending order
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IGenericDataOrder<T> ThenByDescending(Expression<Func<T, object>> selector)
        {
            return new GenericDataOrder<T>(this, selector, GenericDataOrder<T>.Sort.Descending);
        }

        /// <summary>
        /// Build the reversed array that can be used to evaluate ordered selectors
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static GenericDataOrder<T>[] ToArray(GenericDataOrder<T> order)
        {
            GenericDataOrder<T>[] ret = new GenericDataOrder<T>[] { };
            GenericDataOrder<T> currentOrder = order;
            while (currentOrder != null)
            {
                ret = ret.Concat(new GenericDataOrder<T>[] { new GenericDataOrder<T>(null, currentOrder.Selector, currentOrder.Direction) }).ToArray();
                if (currentOrder.HasParent())
                    currentOrder = currentOrder.Parent;
                else
                    currentOrder = null;
            }
            return ret.Reverse().ToArray();
        }
    }
}
