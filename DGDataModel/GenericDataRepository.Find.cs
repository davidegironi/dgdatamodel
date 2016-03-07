#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Data.Entity;

namespace DG.Data.Model
{
    public partial class GenericDataRepository<T, M> : IGenericDataRepository<T, M>
        where T : class
        where M : class
    {
        /// <summary>
        /// Search an item by keys
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find(params object[] keyValues)
        {
            T ret = null;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                //search for keyvalues
                ret = context.Set<T>().Find(keyValues);
            }
            return ret;
        }
    }
}