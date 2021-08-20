#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
#if NETFRAMEWORK
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
#else
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endif
using System.Linq;

namespace DG.Data.Model
{
    public partial class GenericDataRepository<T, M> : IGenericDataRepository<T, M>
        where T : class
        where M : class
    {
        /// <summary>
        /// Data repository helper
        /// </summary>
        public IGenericDataRepositoryHelper<T> Helper { get; private set; }

        /// <summary>
        /// Get or Set context Type
        /// </summary>
        protected internal Type ContextType { get; private set; }

        /// <summary>
        /// Get or Set context creation parameters
        /// </summary>
        protected internal object[] ContextParameters { get; private set; }

        /// <summary>
        /// Get or Set caller of this class
        /// </summary>
        protected internal M BaseModel { get; private set; }

        /// <summary>
        /// Exception String Builder max depth
        /// </summary>
        private const int ExceptionStringBuilderMaxDepth = 10;

        /// <summary>
        /// Repository language
        /// </summary>
        public GenericDataRepositoryLanguageBase languageBase = new GenericDataRepositoryLanguageBase();

        /// <summary>
        /// Instantiate the data repository, you must call the Initialize method
        /// </summary>
        public GenericDataRepository()
        { }

        /// <summary>
        /// Instantiate the data repository
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="contextParameters"></param>
        /// <param name="baseModel"></param>
        public GenericDataRepository(Type contextType, object[] contextParameters, object baseModel)
            : this()
        {
            Initialize(contextType, contextParameters, baseModel);
        }

        /// <summary>
        /// Initialize the repository
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="contextParameters"></param>
        /// <param name="baseModel"></param>
        public virtual void Initialize(Type contextType, object[] contextParameters, object baseModel)
        {
            ContextType = contextType;
            ContextParameters = contextParameters;
            BaseModel = baseModel as M;
            Helper = new GenericDataRepositoryHelper<T>(contextType, contextParameters);
        }

        /// <summary>
        /// Check if EntryEntities are valid
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool CheckEntiryEntriesAreValid(ref string[] errors, params T[] items)
        {
            bool ret = false;


#if NETFRAMEWORK
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                foreach (T item in items)
                {
                    //check validation status
                    if (!context.Entry(item).GetValidationResult().IsValid)
                    {
                        foreach (DbValidationError validationError in context.Entry(item).GetValidationResult().ValidationErrors)
                        {
                            //add errors string
                            errors = errors.Concat(new string[] { validationError.ErrorMessage }).ToArray();
                        }
                        break;
                    }
                }
                if (errors.Length == 0)
                    ret = true;
            }
#else
            //EF core removed the validation of entities
            ret = true;
#endif

            return ret;
        }

        /// <summary>
        /// Check if items has foreing keys dependencies
        /// </summary>
        /// <param name="excludedForeingKeys"></param>
        /// <param name="errors"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool CheckEntiryEntriesHasForeingKeysDependency(string[] excludedForeingKeys, ref string[] errors, params T[] items)
        {
            bool ret = false;

            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                foreach (T item in items)
                {
                    //attach the item to context
                    context.Set<T>().Attach(item);

#if NETFRAMEWORK
                    //loop navigation properties
                    EntityType entityType = ((IObjectContextAdapter)context).ObjectContext.CreateObjectSet<T>().EntitySet.ElementType;
                    foreach (NavigationProperty navigationProperty in entityType.NavigationProperties.Where(x => x.GetDependentProperties().Count() == 0))
                    {
                        if (excludedForeingKeys != null)
                        {
                            if (excludedForeingKeys.Contains(navigationProperty.RelationshipType.Name))
                            {
                                continue;
                            }
                        }
#else
                    //loop navigation properties
                    var entityType = context.Model.FindEntityType(typeof(T));
                    if (excludedForeingKeys == null)
                        excludedForeingKeys = new string[] { };
                    foreach (PropertyInfo navigationProperty in entityType.GetNavigations().Where(x => !x.IsOnDependent && !excludedForeingKeys.Contains(x.ForeignKey.GetConstraintName())).Select(x => x.PropertyInfo))
                    {
#endif
                        var member = context.Entry(item).Member(navigationProperty.Name);
#if NETFRAMEWORK
                        if (member is DbCollectionEntry)
#else
                        if (member is CollectionEntry)
#endif
                        {
                            if (context.Entry(item).Collection(navigationProperty.Name).Query().GetEnumerator().MoveNext())
                            {
                                //add errors string
                                errors = errors.Concat(new string[] { String.Format(languageBase.foreingKeyErrorRaised, navigationProperty.Name) }).ToArray();
                            }
                        }
#if NETFRAMEWORK
                        else if (member is DbReferenceEntry)
#else
                        else if (member is ReferenceEntry)
#endif
                        {
                            if (context.Entry(item).Reference(navigationProperty.Name).Query().GetEnumerator().MoveNext())
                            {
                                //add errors string
                                errors = errors.Concat(new string[] { String.Format(languageBase.foreingKeyErrorRaised, navigationProperty.Name) }).ToArray();
                            }
                        }
                    }
                    if (errors.Length == 0)
                        ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// Build the Exception string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="depth"></param>
        /// <param name="ex"></param>
        protected void GetExceptionString(ref string s, int depth, Exception ex)
        {
            if (ex == null)
            {
                s += "EXCEPTIONERR   : No Exception found." + Environment.NewLine;
            }
            else
            {
                if (depth < ExceptionStringBuilderMaxDepth)
                {
                    if (depth > 0)
                    {
                        s += "INNEREXCEPTION   " + depth + Environment.NewLine;
                    }
                    s += "MESSAGE        : " + ex.Message + Environment.NewLine;
                    s += "SOURCE         : " + ex.Source + Environment.NewLine;
                    s += "TARGETSITE     : " + ex.TargetSite + Environment.NewLine;
                    s += "STACKTRACE     : " + Environment.NewLine + ex.StackTrace.ToString() + Environment.NewLine;
                    if (ex.InnerException != null)
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += Environment.NewLine;
                        depth++;
                        try
                        {
                            GetExceptionString(ref s, depth, ex.InnerException);
                        }
                        catch
                        {
                            s += "EXCEPTIONERR   : There was an error loading this exception." + Environment.NewLine;
                        }
                    }
                }
            }
        }
    }
}