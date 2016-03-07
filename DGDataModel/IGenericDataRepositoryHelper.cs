#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System.Collections.Generic;

namespace DG.Data.Model
{
    public interface IGenericDataRepositoryHelper<T>
        where T : class
    {
        string GetDatabaseName();
        string GetTableName();
        IDictionary<string, object> GetKeyPairs(T item);
        bool ArePropertyValuesChanged(T item, IDictionary<string, object> originalDbPropertyValues);
        IDictionary<string, object> GetPropertyValues(T item);
    }
}
