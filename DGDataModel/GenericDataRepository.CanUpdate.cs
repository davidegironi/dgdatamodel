#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System.Linq;

namespace DG.Data.Model
{
    public partial class GenericDataRepository<T, M> : IGenericDataRepository<T, M>
        where T : class
        where M : class
    {
        /// <summary>
        /// Check if items can be update, return error messages
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual bool CanUpdate(ref string[] errors, params T[] items)
        {
            bool ret = false;
            errors = new string[] { };

            if (items == null)
                return false;
            if (items.Count() == 0)
                return false;

            ret = CheckEntiryEntriesAreValid(ref errors, items);

            return ret;
        }

        /// <summary>
        /// Check if items can be updated
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool CanUpdate(params T[] items)
        {
            string[] errors = new string[] { };
            return CanUpdate(ref errors, items);
        }
    }
}