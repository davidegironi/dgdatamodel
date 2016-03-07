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
        /// Remove items
        /// </summary>
        /// <param name="items"></param>
        public virtual void Remove(params T[] items)
        {
            //remove items
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                foreach (T item in items)
                {
                    //set entity state
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Remove items, do check actions, check foreing keys with excluded foreing keys names, return error messages, if any exception raised build the exception string
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="exceptionTrace"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Remove(ref string[] errors, ref string exceptionTrace, params T[] items)
        {
            return Remove(true, null, ref errors, ref exceptionTrace, items);
        }

        /// <summary>
        /// Remove items, do check actions, eventually check foreing keys with excluded foreing keys names, return error messages, if any exception raised build the exception string
        /// </summary>
        /// <param name="checkForeingKeys"></param>
        /// <param name="excludedForeingKeys"></param>
        /// <param name="errors"></param>
        /// <param name="exceptionTrace"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Remove(bool checkForeingKeys, string[] excludedForeingKeys, ref string[] errors, ref string exceptionTrace, params T[] items)
        {
            bool ret = false;
            exceptionTrace = "";

            try
            {
                //check if item can be removed
                bool canRemove = false;
                if (excludedForeingKeys == null)
                    canRemove = CanRemove(checkForeingKeys, ref errors, items);
                else
                    canRemove = CanRemove(checkForeingKeys, excludedForeingKeys, ref errors, items);
                if (canRemove)
                {
                    try
                    {
                        //try to remove item
                        Remove(items);
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        //build string for exception raised during context commit
                        try
                        {
                            GetExceptionString(ref exceptionTrace, 0, ex);
                        }
                        catch
                        {
                            exceptionTrace = "EXCEPTIONERR   : There was an error loading this exception." + Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //build string for exception raised during check actions
                try
                {
                    GetExceptionString(ref exceptionTrace, 0, ex);
                }
                catch
                {
                    exceptionTrace = "EXCEPTIONERR   : There was an error loading this exception." + Environment.NewLine;
                }
            }

            return ret;
        }

        /// <summary>
        /// Remove items, do check actions, eventually check foreing keys without excluded foreing keys names, return error messages, if any exception raised build the exception string
        /// </summary>
        /// <param name="checkForeingKeys"></param>
        /// <param name="errors"></param>
        /// <param name="exceptionTrace"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Remove(bool checkForeingKeys, ref string[] errors, ref string exceptionTrace, params T[] items)
        {
            return Remove(checkForeingKeys, null, ref errors, ref exceptionTrace, items);
        }
    }
}