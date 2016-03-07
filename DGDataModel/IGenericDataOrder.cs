#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Linq.Expressions;

namespace DG.Data.Model
{
    public interface IGenericDataOrder<T>
        where T : class
    {
        IGenericDataOrder<T> ThenBy(Expression<Func<T, object>> selector);
        IGenericDataOrder<T> ThenByDescending(Expression<Func<T, object>> selector);
    }
}
