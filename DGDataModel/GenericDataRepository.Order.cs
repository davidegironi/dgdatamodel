#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Linq.Expressions;

namespace DG.Data.Model
{
    public partial class GenericDataRepository<T, M> : IGenericDataRepository<T, M>
        where T : class
        where M : class
    {
        /// <summary>
        /// OrderBy in ascending direction using a selector
        /// </summary>
        /// <returns></returns>
        public IGenericDataOrder<T> OrderBy(Expression<Func<T, object>> selector)
        {
            return new GenericDataOrder<T>(this, selector, GenericDataOrder<T>.Sort.Ascending);
        }

        /// <summary>
        /// OrderBy in descending direction using a selector
        /// </summary>
        /// <returns></returns>
        public IGenericDataOrder<T> OrderByDescending(Expression<Func<T, object>> selector)
        {
            return new GenericDataOrder<T>(this, selector, GenericDataOrder<T>.Sort.Descending);
        }
    }
}
