#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace DG.Data.Model
{
    internal class GenericDataRepositoryHelper<T> : IGenericDataRepositoryHelper<T>
        where T : class
    {
        /// <summary>
        /// Get or Set context Type
        /// </summary>
        protected internal Type ContextType { get; private set; }

        /// <summary>
        /// Get or Set context creation parameters
        /// </summary>
        protected internal object[] ContextParameters { get; private set; }

        /// <summary>
        /// Instantiate a data repository helper
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="contextParameters"></param>
        public GenericDataRepositoryHelper(Type contextType, object[] contextParameters)
        {
            this.ContextType = contextType;
            this.ContextParameters = ContextParameters;
        }

        /// <summary>
        /// Get the context database name
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            string database = null;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                string connectString = context.Database.Connection.ConnectionString.ToString();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
                database = builder.InitialCatalog;
            }
            return database;
        }

        /// <summary>
        /// Get the database table mapped by an entity entry
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            string ret = null;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                string sql = (((IObjectContextAdapter)context).ObjectContext).CreateObjectSet<T>().ToTraceString();
                Regex regex = new Regex("FROM (?<table>.*) AS");
                Match match = regex.Match(sql);
                ret = match.Groups["table"].Value;
                //exclude bracket
                MatchCollection matches = new Regex("\\[(?<table>.*?)\\]").Matches(ret);
                Match matchb = matches[matches.Count - 1];
                ret = matchb.Groups["table"].Value;
            }
            return ret;
        }
        
        /// <summary>
        /// Get primary keys of an entity
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetKeyPairs(T item)
        {
            IDictionary<string, object> ret = null;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                //attach the item to context
                context.Set<T>().Attach(item);
                DbEntityEntry entityEntry = context.Entry(item);
                //get key pairs
                ret = (((IObjectContextAdapter)context).ObjectContext).ObjectStateManager.GetObjectStateEntry(entityEntry.Entity).EntityKey.EntityKeyValues.ToDictionary(kv => kv.Key, kv => kv.Value);
                //detach the item to context
                context.Set<T>().Remove(item);
            }
            return ret;
        }

        /// <summary>
        /// Check if underlying/database values of an entity are changed before the entitly load
        /// </summary>
        /// <param name="item"></param>
        /// <param name="originalDbPropertyValues"></param>
        /// <returns></returns>
        public bool ArePropertyValuesChanged(T item, IDictionary<string, object> originalDbPropertyValues)
        {
            bool ret = true;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                //attach the item to context
                context.Set<T>().Attach(item);
                DbEntityEntry entityEntry = context.Entry(item);
                IDictionary<string, object> databaseProperties = DbPropertyValuesToDictionary(context.Entry(item).GetDatabaseValues());
                if (DictionaryEqual<string, object>(originalDbPropertyValues, databaseProperties))
                    ret = false;
                //detach the item to context
                context.Set<T>().Remove(item);
            }
            return ret;
        }

        /// <summary>
        /// Get current DbPropertyValues for and entity and convert it to object
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetPropertyValues(T item)
        {
            IDictionary<string, object> ret = null;
            using (var context = (DbContext)Activator.CreateInstance(ContextType, ContextParameters))
            {
                //attach the item to context
                context.Set<T>().Attach(item);
                DbEntityEntry entityEntry = context.Entry(item);
                //get original values and convert to dictionary
                DbPropertyValues propertyValues = context.Entry(item).CurrentValues.Clone();
                ret = DbPropertyValuesToDictionary(propertyValues);
                //detach the item to context
                context.Set<T>().Remove(item);
            }
            return ret;
        }

        /// <summary>
        /// Compare two Dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private static bool DictionaryEqual<TKey, TValue>(IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            var comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!comparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }

        /// <summary>
        /// Convert DbProperyValues to Dictionary
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static IDictionary<string, object> DbPropertyValuesToDictionary(DbPropertyValues values)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (var propertyName in values.PropertyNames)
            {
                ret.Add(propertyName, values[propertyName]);
            }
            return ret;
        }
    }
}
