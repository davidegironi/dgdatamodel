#region License
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
        /// Check if any item exists using a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            bool ret = false;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                //check for Any item
                if (predicate != null)
                    ret = context.Set<T>().Any<T>(predicate);
                else
                    ret = context.Set<T>().Any<T>();
            }
            return ret;
        }

        /// <summary>
        /// Check if any item exists
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Any(null);
        }
    }
}