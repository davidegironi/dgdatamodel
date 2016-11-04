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
    public interface IGenericDataOrder<T>
        where T : class
    {
        bool SortAscending { get; set; }
        IGenericDataOrder<T> GetParent();
        IGenericDataOrder<T> ThenBy<TKey>(Expression<Func<T, TKey>> selector);
        IGenericDataOrder<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> selector);
        IOrderedQueryable<T> ApplyOrders(IQueryable<T> query);
        IOrderedQueryable<T> ApplyOrderByAscending(IQueryable<T> query);
        IOrderedQueryable<T> ApplyOrderByDescending(IQueryable<T> query);
        IOrderedQueryable<T> ApplyThenByAscending(IOrderedQueryable<T> queryOrdered);
        IOrderedQueryable<T> ApplyThenByDescending(IOrderedQueryable<T> queryOrdered);
    }
}
