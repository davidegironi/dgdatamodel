#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DG.Data.Model
{
    public interface IGenericDataRepository<T, M>
        where T : class
        where M : class
    {
        IGenericDataRepositoryHelper<T> Helper { get; }
        IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, Nullable<int> skip, Nullable<int> take, params Expression<Func<T, object>>[] navigationProperties);
        IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, Nullable<int> skip, Nullable<int> take);
        IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, Nullable<int> take);
        IList<T> List(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby);
        IList<T> List(IGenericDataOrder<T> orderby, Nullable<int> skip, Nullable<int> take);
        IList<T> List(IGenericDataOrder<T> orderby, Nullable<int> take);
        IList<T> List(IGenericDataOrder<T> orderby);
        IList<T> List(Expression<Func<T, bool>> predicate, Nullable<int> take);
        IList<T> List(Expression<Func<T, bool>> predicate);
        IList<T> List();
        void Add(params T[] items);
        bool Add(ref string[] errors, ref string exceptionTrace, params T[] items);
        void Update(params T[] items);
        bool Update(ref string[] errors, ref string exceptionTrace, params T[] items);
        void Remove(params T[] items);
        bool Remove(ref string[] errors, ref string exceptionTrace, params T[] items);
        bool Remove(bool checkForeingKeys, string[] excludedForeingKeys, ref string[] errors, ref string exceptionTrace, params T[] items);
        bool Remove(bool checkForeingKeys, ref string[] errors, ref string exceptionTrace, params T[] items);
        bool CanAdd(ref string[] errors, params T[] items);
        bool CanAdd(params T[] items);
        bool CanUpdate(ref string[] errors, params T[] items);
        bool CanUpdate(params T[] items);
        bool CanRemove(ref string[] errors, params T[] items);
        bool CanRemove(params T[] items);
        bool CanRemove(bool checkForeingKeys, string[] excludedForeingKeys, ref string[] errors, params T[] items);
        bool CanRemove(bool checkForeingKeys, ref string[] errors, params T[] items);
        IGenericDataOrder<T> OrderBy<TKey>(Expression<Func<T, TKey>> selector);
        IGenericDataOrder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> selector);
        T Find(params object[] keyValues);
        T FirstOrDefault(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby, params Expression<Func<T, object>>[] navigationProperties);
        T FirstOrDefault(Expression<Func<T, bool>> predicate, IGenericDataOrder<T> orderby);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(IGenericDataOrder<T> orderby);
        T FirstOrDefault();
        bool Any(Expression<Func<T, bool>> predicate);
        bool Any();
        int Count(Expression<Func<T, bool>> predicate);
        int Count();
        long LongCount(Expression<Func<T, bool>> predicate);
        long LongCount();
        object Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> selector);
        object Sum(Expression<Func<T, object>> selector);
        decimal Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        decimal Sum(Expression<Func<T, decimal>> selector);
        Nullable<decimal> Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<decimal>>> selector);
        Nullable<decimal> Sum(Expression<Func<T, Nullable<decimal>>> selector);
        double Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector);
        double Sum(Expression<Func<T, double>> selector);
        Nullable<double> Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<double>>> selector);
        Nullable<double> Sum(Expression<Func<T, Nullable<double>>> selector);
        float Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        float Sum(Expression<Func<T, float>> selector);
        Nullable<float> Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<float>>> selector);
        Nullable<float> Sum(Expression<Func<T, Nullable<float>>> selector);
        int Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        int Sum(Expression<Func<T, int>> selector);
        Nullable<int> Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<int>>> selector);
        Nullable<int> Sum(Expression<Func<T, Nullable<int>>> selector);
        long Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        long Sum(Expression<Func<T, long>> selector);
        Nullable<long> Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<long>>> selector);
        Nullable<long> Sum(Expression<Func<T, Nullable<long>>> selector);
        object Average(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> selector);
        object Average(Expression<Func<T, object>> selector);
        decimal Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        decimal Average(Expression<Func<T, decimal>> selector);
        Nullable<decimal> Average(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<decimal>>> selector);
        Nullable<decimal> Average(Expression<Func<T, Nullable<decimal>>> selector);
        double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector);
        double Average(Expression<Func<T, double>> selector);
        Nullable<double> Average(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<double>>> selector);
        Nullable<double> Average(Expression<Func<T, Nullable<double>>> selector);
        float Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        float Average(Expression<Func<T, float>> selector);
        Nullable<float> Average(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<float>>> selector);
        Nullable<float> Average(Expression<Func<T, Nullable<float>>> selector);
        double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        double Average(Expression<Func<T, int>> selector);
        Nullable<double> Average(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<int>>> selector);
        Nullable<double> Average(Expression<Func<T, Nullable<int>>> selector);
        double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        double Average(Expression<Func<T, long>> selector);
        Nullable<double> Average(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<long>>> selector);
        Nullable<double> Average(Expression<Func<T, Nullable<long>>> selector);
        object Min(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> selector);
        object Min(Expression<Func<T, object>> selector);
        decimal Min(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        decimal Min(Expression<Func<T, decimal>> selector);
        Nullable<decimal> Min(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<decimal>>> selector);
        Nullable<decimal> Min(Expression<Func<T, Nullable<decimal>>> selector);
        double Min(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector);
        double Min(Expression<Func<T, double>> selector);
        Nullable<double> Min(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<double>>> selector);
        Nullable<double> Min(Expression<Func<T, Nullable<double>>> selector);
        float Min(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        float Min(Expression<Func<T, float>> selector);
        Nullable<float> Min(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<float>>> selector);
        Nullable<float> Min(Expression<Func<T, Nullable<float>>> selector);
        int Min(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        int Min(Expression<Func<T, int>> selector);
        Nullable<int> Min(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<int>>> selector);
        Nullable<int> Min(Expression<Func<T, Nullable<int>>> selector);
        long Min(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        long Min(Expression<Func<T, long>> selector);
        Nullable<long> Min(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<long>>> selector);
        Nullable<long> Min(Expression<Func<T, Nullable<long>>> selector);
        object Max(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> selector);
        object Max(Expression<Func<T, object>> selector);
        decimal Max(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        decimal Max(Expression<Func<T, decimal>> selector);
        Nullable<decimal> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<decimal>>> selector);
        Nullable<decimal> Max(Expression<Func<T, Nullable<decimal>>> selector);
        double Max(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector);
        double Max(Expression<Func<T, double>> selector);
        Nullable<double> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<double>>> selector);
        Nullable<double> Max(Expression<Func<T, Nullable<double>>> selector);
        float Max(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        float Max(Expression<Func<T, float>> selector);
        Nullable<float> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<float>>> selector);
        Nullable<float> Max(Expression<Func<T, Nullable<float>>> selector);
        int Max(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        int Max(Expression<Func<T, int>> selector);
        Nullable<int> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<int>>> selector);
        Nullable<int> Max(Expression<Func<T, Nullable<int>>> selector);
        long Max(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        long Max(Expression<Func<T, long>> selector);
        Nullable<long> Max(Expression<Func<T, bool>> predicate, Expression<Func<T, Nullable<long>>> selector);
        Nullable<long> Max(Expression<Func<T, Nullable<long>>> selector);
    }
}
