using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;


/*
 * install Microsoft.EntityFrameworkCore.Relational
 */

namespace Cpa.Application.Extensions.EfHelper
{
    public static class EfHelper
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate.Compile());
            }
            else
            {
                return source;
            }
        }

        /*https://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property*/
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }

        /*https://stackoverflow.com/a/40573585/2948523*/
        //public static IQueryable<TSource> DistinctBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        //{
        //    return source.GroupBy(keySelector).Select(x => x.First());
        //}

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            else
            {
                return source;
            }
        }

        public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
        {
            if (condition)
            {
                return source.Include(navigationPropertyPath);
            }
            else
            {
                return source;
            }
        }

        /*https://stackoverflow.com/a/31162909/2948523*/
        public static bool AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class
        {
            var exists = dbSet.Any(predicate);
            if (!exists)
            {
                dbSet.Add(entity);
                return true;
            }
            else
            {
                return false;
            }
        }

        /*if duplicate in db, it will be added*/
        public static void AddIfNotExists<T>(this DbSet<T> dbSet, IEnumerable<T> entitys, Func<T, Expression<Func<T, bool>>> func, bool checkDuplicatesInSourceList = false) where T : class
        {
            if (!checkDuplicatesInSourceList)
            {
                foreach (var entity in entitys)
                {
                    dbSet.AddIfNotExists(entity, func(entity));
                }
                return;
            }

            List<T> list = new List<T>();
            foreach (var entity in entitys)
            {
                if (list.Any(func(entity).Compile()))
                {
                    continue;
                }
                dbSet.AddIfNotExists(entity, func(entity));
                list.Add(entity);
            }
        }

        /*bulk delete https://stackoverflow.com/questions/41960215/how-do-i-delete-multiple-rows-in-entity-framework-core*/
    }
}
