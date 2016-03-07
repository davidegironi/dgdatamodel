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
        /// Check if items can be removed, check foreing keys without excluded foreing keys names, return error messages
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool CanRemove(ref string[] errors, params T[] items)
        {
            return CanRemove(true, null, ref errors, items);
        }

        /// <summary>
        /// Check if items can be removed, eventually check foreing keys without excluded foreing keys names
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool CanRemove(params T[] items)
        {
            string[] errors = new string[] { };
            return CanRemove(true, null, ref errors, items);
        }

        /// <summary>
        /// Check if items can be removed, eventually check foreing keys with excluded foreing keys names, return error messages
        /// </summary>
        /// <param name="checkForeingKeys"></param>
        /// <param name="excludedForeingKeys"></param>
        /// <param name="errors"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual bool CanRemove(bool checkForeingKeys, string[] excludedForeingKeys, ref string[] errors, params T[] items)
        {
            bool ret = false;
            errors = new string[] { };

            if (items == null)
                return true;
            if (items.Count() == 0)
                return true;

            ret = CheckEntiryEntriesAreValid(ref errors, items);
            if (ret && checkForeingKeys)
                ret = CheckEntiryEntriesHasForeingKeysDependency(excludedForeingKeys, ref errors, items);

            return ret;
        }

        /// <summary>
        /// Check if items can be removed, eventually check foreing keys without excluded foreing keys names, return error messages
        /// </summary>
        /// <param name="checkForeingKeys"></param>
        /// <param name="errors"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool CanRemove(bool checkForeingKeys, ref string[] errors, params T[] items)
        {
            return CanRemove(checkForeingKeys, null, ref errors, items);
        }
    }
}