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
        /// Count items using a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            int ret = 0;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                //count items
                if (predicate != null)
                    ret = context.Set<T>().Count<T>(predicate);
                else
                    ret = context.Set<T>().Count<T>();
            }
            return ret;
        }

        /// <summary>
        /// Count all items
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Count(null);
        }
    }
}