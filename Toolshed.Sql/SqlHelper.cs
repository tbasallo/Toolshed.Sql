﻿using System.Data;

using FastMember;

using Microsoft.Data.SqlClient;

namespace Toolshed.Sql;

public static class SqlHelper
{
    /// <summary>
    /// The command timeout that will be used for all commands if set. This is a static property and will apply to ALL commands using it for the rest of its life
    /// unless changed. Use with care.
    /// If you're unsure what that means, ask someone.
    /// </summary>
    public static int? CommandTimeout { get; set; }


    public static void LogToConsoleStringLengths<T>(List<T> data, bool stopWhenDone = true, Type[]? ignoreColumnAttributes = null)
    {
        foreach (var item in typeof(T).GetProperties())
        {
            if (ignoreColumnAttributes != null)
            {
                var d1 = item.CustomAttributes;
                if (d1.Any(x => ignoreColumnAttributes.Contains(x.AttributeType)))
                {
                    continue;
                }
            }

            if (item.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreBulkImportAttribute)))
            {
                continue;
            }

            if (item.PropertyType == typeof(string))
            {
                var longest = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    if (item.GetValue(data[i]) is string l)
                    {
                        longest = longest > l.Length ? longest : l.Length;
                    }
                }

                Console.WriteLine($"{item.Name}: {longest}");
            }
        }

        if (stopWhenDone)
        {
            Console.WriteLine("Done - click a key to continue");
            Console.ReadLine();
        }
    }


    public static void BulkInsert<T>(SqlConnection connection, string tableName, IEnumerable<T> data, int batchSize = 5000, int commandTimeout = 3600, Type[]? ignoreColumnAttributes = null)
    {
        using var bc = new SqlBulkCopy(connection)
        {
            BatchSize = batchSize,
            BulkCopyTimeout = commandTimeout,
            DestinationTableName = tableName,
            EnableStreaming = true
        };

        foreach (var item in typeof(T).GetProperties())
        {
            if (item.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreBulkImportAttribute)))
            {
                continue;
            }
            if (ignoreColumnAttributes != null)
            {
                var d1 = item.CustomAttributes;
                if (d1.Any(x => ignoreColumnAttributes.Contains(x.AttributeType)))
                {
                    continue;
                }
            }

            bc.ColumnMappings.Add(item.Name, item.Name);
        }

        using var reader = ObjectReader.Create(data);

        bc.WriteToServer(reader);
    }
    public static void BulkInsert<T>(SqlConnection connection, SqlTransaction sqlTransaction, string tableName, IEnumerable<T> data, int batchSize = 5000, int commandTimeout = 3600, Type[]? ignoreColumnAttributes = null)
    {
        using var bc = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, sqlTransaction)
        {
            BatchSize = batchSize,
            BulkCopyTimeout = commandTimeout,
            DestinationTableName = tableName,
            EnableStreaming = true
        };

        foreach (var item in typeof(T).GetProperties())
        {
            if (item.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreBulkImportAttribute)))
            {
                continue;
            }
            if (ignoreColumnAttributes != null)
            {
                var d1 = item.CustomAttributes;
                if (d1.Any(x => ignoreColumnAttributes.Contains(x.AttributeType)))
                {
                    continue;
                }
            }

            bc.ColumnMappings.Add(item.Name, item.Name);
        }

        using var reader = ObjectReader.Create(data);

        bc.WriteToServer(reader);
    }

    public static async Task BulkInsertAsync<T>(SqlConnection connection, string tableName, List<T> data, int batchSize = 5000, int commandTimeout = 3600, Type[]? ignoreColumnAttributes = null)
    {
        using var bc = new SqlBulkCopy(connection)
        {
            BatchSize = batchSize,
            BulkCopyTimeout = commandTimeout,
            DestinationTableName = tableName,
            EnableStreaming = true
        };

        foreach (var item in typeof(T).GetProperties())
        {
            if (item.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreBulkImportAttribute)))
            {
                continue;
            }
            if (ignoreColumnAttributes != null)
            {
                var d1 = item.CustomAttributes;
                if (d1.Any(x => ignoreColumnAttributes.Contains(x.AttributeType)))
                {
                    continue;
                }
            }

            bc.ColumnMappings.Add(item.Name, item.Name);
        }

        using var reader = ObjectReader.Create(data);

        await bc.WriteToServerAsync(reader);
    }
    public static async Task BulkInsertAsync<T>(SqlConnection connection, SqlTransaction sqlTransaction, string tableName, List<T> data, int batchSize = 5000, int commandTimeout = 3600, Type[]? ignoreColumnAttributes = null)
    {
        using var bc = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, sqlTransaction)
        {
            BatchSize = batchSize,
            BulkCopyTimeout = commandTimeout,
            DestinationTableName = tableName,
            EnableStreaming = true
        };

        foreach (var item in typeof(T).GetProperties())
        {
            if (item.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreBulkImportAttribute)))
            {
                continue;
            }
            if (ignoreColumnAttributes != null)
            {
                var d1 = item.CustomAttributes;
                if (d1.Any(x => ignoreColumnAttributes.Contains(x.AttributeType)))
                {
                    continue;
                }
            }

            bc.ColumnMappings.Add(item.Name, item.Name);
        }

        using var reader = ObjectReader.Create(data);

        await bc.WriteToServerAsync(reader);
    }
    public static async Task BulkInsertAsync<T>(SqlConnection connection, string tableName, ObjectReader objectReader, int batchSize = 5000, int commandTimeout = 3600, Type[]? ignoreColumnAttributes = null, string[]? ignoreColumns = null)
    {
        using var bc = new SqlBulkCopy(connection)
        {
            BatchSize = batchSize,
            BulkCopyTimeout = commandTimeout,
            DestinationTableName = tableName,
            EnableStreaming = true
        };

        foreach (var item in typeof(T).GetProperties())
        {
            if (item.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreBulkImportAttribute)))
            {
                continue;
            }
            if (ignoreColumnAttributes != null)
            {
                var d1 = item.CustomAttributes;
                if (d1.Any(x => ignoreColumnAttributes.Contains(x.AttributeType)))
                {
                    continue;
                }
            }

            if (ignoreColumns != null)
            {
                if (ignoreColumns.Contains(item.Name))
                {
                    continue;
                }
            }

            bc.ColumnMappings.Add(item.Name, item.Name);
        }

        await bc.WriteToServerAsync(objectReader);
    }
    public static async Task BulkInsertAsync<T>(SqlConnection connection, SqlTransaction sqlTransaction, string tableName, ObjectReader objectReader, int batchSize = 5000, int commandTimeout = 3600, Type[]? ignoreColumnAttributes = null, string[]? ignoreColumns = null)
    {
        using var bc = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, sqlTransaction)
        {
            BatchSize = batchSize,
            BulkCopyTimeout = commandTimeout,
            DestinationTableName = tableName,
            EnableStreaming = true
        };

        foreach (var item in typeof(T).GetProperties())
        {
            if (item.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreBulkImportAttribute)))
            {
                continue;
            }
            if (ignoreColumnAttributes != null)
            {
                var d1 = item.CustomAttributes;
                if (d1.Any(x => ignoreColumnAttributes.Contains(x.AttributeType)))
                {
                    continue;
                }
            }

            if (ignoreColumns != null)
            {
                if (ignoreColumns.Contains(item.Name))
                {
                    continue;
                }
            }

            bc.ColumnMappings.Add(item.Name, item.Name);
        }

        await bc.WriteToServerAsync(objectReader);
    }




    public static void UseDatabase(this SqlConnection connection, string databaseName)
    {
        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }
        connection.ChangeDatabase(databaseName);
    }

    public static int TruncateTable(string tableName, SqlConnection connection, SqlTransaction? transaction = null)
    {
        return ExecuteNonQuery($"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{tableName}') AND type in (N'U')) TRUNCATE TABLE {tableName}", connection, transaction);
    }
    public async static Task<int> TruncateTableAsync(string tableName, SqlConnection connection, SqlTransaction? transaction = null)
    {
        return await ExecuteNonQueryAsync($"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{tableName}') AND type in (N'U')) TRUNCATE TABLE {tableName}", connection, transaction);
    }

    public static int DropExistingTable(string tableName, SqlConnection connection, SqlTransaction? transaction = null)
    {
        return ExecuteNonQuery($"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{tableName}') AND type in (N'U')) DROP TABLE {tableName}", connection, transaction);
    }

    public static int RenameTable(string oldName, string newName, SqlConnection connection, SqlTransaction? transaction = null)
    {
        return ExecuteNonQuery(string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'", oldName, newName), connection, transaction);
    }



    public static T? ExecuteScalar<T>(string sql, SqlConnection connection)
    {
        using SqlCommand command = new SqlCommand(sql, connection);
        command.CommandType = CommandType.Text;
        command.CommandTimeout = CommandTimeout.GetValueOrDefault(command.CommandTimeout);
        var result = command.ExecuteScalar();
        if (result is null)
        {
            return default;
        }

        return (T)result;
    }
    public async static Task<T?> ExecuteScalarAsync<T>(string sql, SqlConnection connection)
    {
        using SqlCommand command = new SqlCommand(sql, connection);
        command.CommandType = CommandType.Text;
        command.CommandTimeout = CommandTimeout.GetValueOrDefault(command.CommandTimeout);
        var result = await command.ExecuteScalarAsync();
        if (result is null)
        {
            return default;
        }

        return (T)result;
    }

    public static T? ExecuteScalar<T>(string sql, SqlConnection connection, SqlTransaction transaction)
    {
        using SqlCommand command = new SqlCommand(sql, connection, transaction);
        command.CommandType = CommandType.Text;
        command.CommandTimeout = CommandTimeout.GetValueOrDefault(command.CommandTimeout);
        var result = command.ExecuteScalar();

        if (result is null)
        {
            return default;
        }

        return (T)result;
    }
    public async static Task<T?> ExecuteScalarAsync<T>(string sql, SqlConnection connection, SqlTransaction transaction)
    {
        using SqlCommand command = new SqlCommand(sql, connection, transaction);
        command.CommandType = CommandType.Text;
        command.CommandTimeout = CommandTimeout.GetValueOrDefault(command.CommandTimeout);
        var result = await command.ExecuteScalarAsync();

        if (result is null)
        {
            return default;
        }

        return (T)result;
    }


    public static int ExecuteNonQuery(string sql, SqlConnection connection, SqlTransaction? transaction = null, int? commandTimeout = null)
    {
        if (transaction is null)
        {
            return ExecuteNonQuery(sql, connection, CommandType.Text, commandTimeout);
        }
        return ExecuteNonQuery(sql, connection, transaction, CommandType.Text, commandTimeout);
    }
    public static int ExecuteNonQuery(string sql, SqlConnection connection, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection);
        command.CommandType = commandType;
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        return command.ExecuteNonQuery();
    }
    public static int ExecuteNonQuery(string sql, SqlConnection connection, SqlTransaction transaction, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection, transaction);
        command.CommandType = commandType;
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        return command.ExecuteNonQuery();
    }
    public static int ExecuteNonQuery(string sql, SqlConnection connection, SqlParameter[] parameters, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection);
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        command.CommandType = commandType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);
        return command.ExecuteNonQuery();
    }
    public static int ExecuteNonQuery(string sql, SqlConnection connection, SqlTransaction transaction, SqlParameter[] parameters, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection, transaction);
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        command.CommandType = commandType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);
        return command.ExecuteNonQuery();
    }


    public async static Task<int> ExecuteNonQueryAsync(string sql, SqlConnection connection, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection);
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        command.CommandType = commandType;
        return await command.ExecuteNonQueryAsync();
    }
    public async static Task<int> ExecuteNonQueryAsync(string sql, SqlConnection connection, SqlTransaction? transaction = null, int? commandTimeout = null)
    {
        if (transaction == null)
        {
            return await ExecuteNonQueryAsync(sql, connection, CommandType.Text, commandTimeout);
        }
        return await ExecuteNonQueryAsync(sql, connection, transaction, CommandType.Text, commandTimeout);
    }
    public async static Task<int> ExecuteNonQueryAsync(string sql, SqlConnection connection, SqlTransaction transaction, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection, transaction);
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        command.CommandType = commandType;
        return await command.ExecuteNonQueryAsync();
    }
    public async static Task<int> ExecuteNonQueryAsync(string sql, SqlConnection connection, SqlParameter[] parameters, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection);
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        command.CommandType = commandType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);
        return await command.ExecuteNonQueryAsync();
    }
    public async static Task<int> ExecuteNonQueryAsync(string sql, SqlConnection connection, SqlTransaction transaction, SqlParameter[] parameters, CommandType commandType, int? commandTimeout = null)
    {
        using SqlCommand command = new SqlCommand(sql, connection, transaction);
        if (commandTimeout.HasValue || CommandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.GetValueOrDefault(CommandTimeout.GetValueOrDefault(command.CommandTimeout));
        }
        command.CommandType = commandType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);
        return await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// This checks if a given statement is true. It checks for a return value of 1 or 0 NOT a null value.
    /// You should use EXISTS to minimize the amount of work that SQL does. Something like:
    /// SELECT CASE WHEN EXISTS(SELECT 1 FROM your.table WHERE yourCriteria = 'something') THEN 1 ELSE 0 END");
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="sql"></param>
    /// <returns></returns>
    public async static Task<bool> ExistsAsync(string connectionString, string sql)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = new SqlCommand(sql, connection);

        command.CommandType = CommandType.Text;
        if (CommandTimeout.HasValue)
        {
            command.CommandTimeout = CommandTimeout.GetValueOrDefault();
        }

        var result = await command.ExecuteScalarAsync();
        return result is not null && (int)result == 1;
    }

}
