﻿#region License
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
        /// Add items
        /// </summary>
        /// <param name="items"></param>
        public virtual void Add(params T[] items)
        {
            //add items
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                foreach (T item in items)
                {
                    //set entity state
                    context.Entry(item).State = EntityState.Added;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Add items, do check actions, return error messages, if any exception raised build the exception string
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="exceptionTrace"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Add(ref string[] errors, ref string exceptionTrace, params T[] items)
        {
            bool ret = false;
            exceptionTrace = "";

            try
            {
                //check if item can be added
                if (CanAdd(ref errors, items))
                {
                    try
                    {
                        //try to add item
                        Add(items);
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
    }
}