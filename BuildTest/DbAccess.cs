using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildTest.Config;
using BuildTest.Database;
using BuildTest.Uow;
using System.Data.Common;

namespace BuildTest
{
    public static class DbAccess
    {
        private static async void PrepareCommandAsync(DbCommand cmd, string dbInstance)
        {
            if (cmd.Transaction == null && cmd.Connection != null)
            {
                UnitOfWork.UnitOfWorlImpl unitOfWork = UnitOfWork.UnitOfWorlImpl.Current;
                if (unitOfWork == null)
                {
                    return;
                }
                if (unitOfWork.DbTransaction == null)
                {
                    var instance = DbInstance.Get(dbInstance);
                    var con = instance.CreateConnection();
                    con.ConnectionString = instance.ConnectString;
                    await con.OpenAsync();
                    unitOfWork.DbTransaction = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = unitOfWork.DbTransaction;

                }
                else
                {
                    if (unitOfWork.DbTransaction.Connection.State != ConnectionState.Open)
                    {
                        await unitOfWork.DbTransaction.Connection.OpenAsync();
                        unitOfWork.DbTransaction = unitOfWork.DbTransaction.Connection.BeginTransaction();
                        cmd.Connection = unitOfWork.DbTransaction.Connection;
                        cmd.Transaction = unitOfWork.DbTransaction;
                    }
                }
            }
        }

        private static void PrepareCommand(DbCommand cmd, string dbInstance)
        {
            if (cmd.Transaction == null)
            {
                UnitOfWork.UnitOfWorlImpl unitOfWork = UnitOfWork.UnitOfWorlImpl.Current;
                if (unitOfWork == null)
                {
                    return;
                }
                if (unitOfWork.DbTransaction == null)
                {
                    var instance = DbInstance.Get(dbInstance);
                    var con = instance.CreateConnection();
                    con.ConnectionString = instance.ConnectString;
                    con.Open();
                    unitOfWork.DbTransaction = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = unitOfWork.DbTransaction;

                }
                else
                {
                    if (unitOfWork.DbTransaction.Connection.State != ConnectionState.Open)
                    {
                        unitOfWork.DbTransaction.Connection.Open();
                        unitOfWork.DbTransaction = unitOfWork.DbTransaction.Connection.BeginTransaction();
                        cmd.Connection = unitOfWork.DbTransaction.Connection;
                        cmd.Transaction = unitOfWork.DbTransaction;
                    }
                }
            }
        }

        public static int ExecuteNonQuery(DbCommand cmd, string dbInstance)
        {
            PrepareCommand(cmd, dbInstance);
            return cmd.ExecuteNonQuery();
        }

        public static DbDataReader ExecuteReader(DbCommand cmd, string dbInstance)
        {
            PrepareCommand(cmd, dbInstance);
            return cmd.ExecuteReader();
        }

        public static object ExecuteScalar(DbCommand cmd, string dbInstance)
        {
            PrepareCommand(cmd, dbInstance);
            return cmd.ExecuteScalar();
        }

        public static async Task<object> ExecuteScalarAsync(DbCommand cmd, string dbInstance)
        {
            PrepareCommandAsync(cmd, dbInstance);
            return await cmd.ExecuteScalarAsync();
        }

        public static async Task<DbDataReader> ExecuteReaderAsync(DbCommand cmd, string dbInstance)
        {
            PrepareCommandAsync(cmd, dbInstance);
            return await cmd.ExecuteReaderAsync();
        }

        public static async Task<int> ExecuteNonQueryAsync(DbCommand cmd, string dbInstance)
        {
            PrepareCommandAsync(cmd, dbInstance);
            return await cmd.ExecuteNonQueryAsync();
        }



    }
}
