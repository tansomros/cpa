using BigLion.CPA.Application.Extensions.EfHelper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Reflection;


/*
 * install Microsoft.EntityFrameworkCore.Relational
 */

namespace BigLion.CPA.Application.Extensions.EfHelper
{
    public static class EfSqlHelper
    {
        /*
         * if want dynamic mapping 
         * IEnumerable<T> RawSqlQuery<T>(this DbContext context, string query, Func<DbDataReader, T> map, params object[] parameters)
         * 
         * DbDataReader to list https://stackoverflow.com/questions/1464883/how-can-i-easily-convert-datareader-to-listt
         * column name and types https://stackoverflow.com/a/27200892/2948523
         * https://stackoverflow.com/questions/681653/can-you-get-the-column-names-from-a-sqldatareader
         */
        private class PropertyMapp
        {
            public required string Name { get; set; }
            public Type? Type { get; set; }
            public bool IsSame(PropertyMapp mapp)
            {
                if (mapp == null)
                {
                    return false;
                }
                bool same = mapp.Name == Name && mapp.Type == Type;
                return same;
            }
            public bool IsSame(PropertyMapp mapp, bool InnoreCase)
            {
                if (mapp == null)
                {
                    return false;
                }
                bool same = mapp.Name.ToLower() == Name.ToLower() && mapp.Type == Type;
                return same;
            }
        }

        /*https://dapper-tutorial.net/knowledge-base/46566756/how-do-i-get-an-idbtransaction-from-an-idbcontext-*/
        public static DbTransaction GetDbTransaction(this IDbContextTransaction source)
        {
            return ((IInfrastructure<DbTransaction>)source).Instance;
        }


        /*https://stackoverflow.com/questions/46163254/how-to-get-scalar-value-from-a-sql-statement-in-a-net-core-application*/
        public static object? ExecuteScalar(this DbContext context, string sql, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            object? value = context.Database.ExecuteScalar(sql, parameters, commandType, commandTimeOutInSeconds);
            return value;
        }

        public static object? ExecuteScalar(this DatabaseFacade database, string sql, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            object? value;
            using (var cmd = database.GetDbConnection().CreateCommand())
            {
                if (cmd.Connection!.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                var currentTransaction = database.CurrentTransaction;
                if (currentTransaction != null)
                {
                    cmd.Transaction = currentTransaction.GetDbTransaction();
                }
                cmd.CommandText = sql;
                cmd.CommandType = commandType;
                if (commandTimeOutInSeconds != null)
                {
                    cmd.CommandTimeout = (int)commandTimeOutInSeconds;
                }
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                value = cmd.ExecuteScalar();
            }
            return value;
        }

        public static int ExecuteNonQuery(this DbContext context, string command, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            int value = context.Database.ExecuteNonQuery(command, parameters, commandType, commandTimeOutInSeconds);
            return value;
        }

        public static int ExecuteNonQuery(this DatabaseFacade database, string command, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            using (var cmd = database.GetDbConnection().CreateCommand())
            {
                if (cmd.Connection!.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                var currentTransaction = database.CurrentTransaction;
                if (currentTransaction != null)
                {
                    cmd.Transaction = currentTransaction.GetDbTransaction();
                }
                cmd.CommandText = command;
                cmd.CommandType = commandType;
                if (commandTimeOutInSeconds != null)
                {
                    cmd.CommandTimeout = (int)commandTimeOutInSeconds;
                }
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// TSource should be included in DbContext as DbSet<TSource> or DbQuery<T>
        /// </summary>
        public static IQueryable<TSource> FromSqlRaw<TSource>(this DbContext db, string sql, params object[] parameters) where TSource : class
        {
            var item = db.Set<TSource>().FromSqlRaw(sql, parameters);
            return item;
        }
        public static IEnumerable<T> FromSqlQuerySetUTF8<T>(this DatabaseFacade database, string query, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null) where T : new ()
        {
            query = "SET CLIENT_ENCODING TO 'UTF8'; " + query;
            return database.FromSqlQuery<T>(query);
        }
        public static IEnumerable<dynamic> FromSqlQuerySetUTF8(this DatabaseFacade database, string query, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            DataTable table = new DataTable();
            query = "SET CLIENT_ENCODING TO 'UTF8'; " + query;
            using (var command = database.GetDbConnection().CreateCommand())
            {
                if (command.Connection!.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                var currentTransaction = database.CurrentTransaction;
                if (currentTransaction != null)
                {
                    command.Transaction = currentTransaction.GetDbTransaction();
                }
                command.CommandText = query;
                command.CommandType = commandType;
                if (commandTimeOutInSeconds != null)
                {
                    command.CommandTimeout = (int)commandTimeOutInSeconds;
                }
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                using (var reader = command.ExecuteReader())
                {
                    var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    foreach(IDataRecord recore in reader as IEnumerable)
                    {
                        var expando = new ExpandoObject() as IDictionary<string, object>;
                        foreach (var name in names)
                            expando[name] = recore[name];
                        yield return expando;
                    }
                }
            }
        }
        public static IEnumerable<T> FromSqlQuery<T>(this DbContext context, string query, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null) where T : new()
        {
            return context.Database.FromSqlQuery<T>(query, parameters, commandType, commandTimeOutInSeconds);
        }
        public static IEnumerable<T> FromSqlQuery<T>(this DatabaseFacade database, string query, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null) where T : new()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase;
            List<PropertyMapp> entityFields = (from PropertyInfo aProp in typeof(T).GetProperties(flags)
                                               select new PropertyMapp
                                               {
                                                   Name = aProp.Name,
                                                   Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                               }).ToList();
            List<PropertyMapp> dbDataReaderFields = new List<PropertyMapp>();
            List<PropertyMapp>? commonFields = null;

            using (var command = database.GetDbConnection().CreateCommand())
            {
                if (command.Connection!.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                var currentTransaction = database.CurrentTransaction;
                if (currentTransaction != null)
                {
                    command.Transaction = currentTransaction.GetDbTransaction();
                }
                command.CommandText = query;
                command.CommandType = commandType;
                if (commandTimeOutInSeconds != null)
                {
                    command.CommandTimeout = (int)commandTimeOutInSeconds;
                }
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        if (commonFields == null)
                        {
                            for (int i = 0; i < result.FieldCount; i++)
                            {
                                dbDataReaderFields.Add(new PropertyMapp { Name = result.GetName(i), Type = result.GetFieldType(i) });
                            }
                            commonFields = entityFields.Where(x => dbDataReaderFields.Any(d => d.IsSame(x, true))).Select(x => x).ToList();
                        }

                        var entity = new T();
                        foreach (var aField in commonFields)
                        {
                            PropertyInfo? propertyInfos = entity.GetType().GetProperty(aField.Name);
                            var value = result[aField.Name] == DBNull.Value ? null : result[aField.Name]; //if field is nullable
                            propertyInfos?.SetValue(entity, value, null);
                        }
                        yield return entity;
                    }
                }
            }
        }
        public static IEnumerable<object> FromSqlQuery(this DatabaseFacade database, string query, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            //DataTable table = new DataTable();
            using (var command = database.GetDbConnection().CreateCommand())
            {
                if (command.Connection!.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                var currentTransaction = database.CurrentTransaction;
                if (currentTransaction != null)
                {
                    command.Transaction = currentTransaction.GetDbTransaction();
                }
                command.CommandText = query;
                command.CommandType = commandType;
                if (commandTimeOutInSeconds != null)
                {
                    command.CommandTimeout = (int)commandTimeOutInSeconds;
                }
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                using (var reader = command.ExecuteReader())
                {
                    var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    foreach (IDataRecord recore in reader as IEnumerable)
                    {
                        var expando = new ExpandoObject() as IDictionary<string, object>;
                        foreach (var name in names)
                            expando[name] = recore[name];
                        yield return expando;
                    }
                }
                //using (var result = command.ExecuteReader())
                //{
                //    table.Load(result);
                //    yield return JsonConvert.SerializeObject(table);
                //}
            }
        }

        /*
         * https://entityframeworkcore.com/knowledge-base/35631903/raw-sql-query-without-dbset---entity-framework-core
         */
        public static IEnumerable<T> FromSqlQuery<T>(this DbContext context, string query, Func<DbDataReader, T> map, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            return context.Database.FromSqlQuery(query, map, parameters, commandType, commandTimeOutInSeconds);
        }

        public static IEnumerable<T> FromSqlQuery<T>(this DatabaseFacade database, string query, Func<DbDataReader, T> map, List<DbParameter>? parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null)
        {
            using (var command = database.GetDbConnection().CreateCommand())
            {
                if (command.Connection!.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                var currentTransaction = database.CurrentTransaction;
                if (currentTransaction != null)
                {
                    command.Transaction = currentTransaction.GetDbTransaction();
                }
                command.CommandText = query;
                command.CommandType = commandType;
                if (commandTimeOutInSeconds != null)
                {
                    command.CommandTimeout = (int)commandTimeOutInSeconds;
                }
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        yield return map(result);
                    }
                }
            }
        }
    }
}
