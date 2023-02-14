﻿using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ATM_DAL.Database
{
    public class AtmDbContext : IDisposable
    {
        private readonly string _connString;

        private SqlConnection _dbConnection = null;

        public AtmDbContext() : this(@"Data Source=LAPTOP-AI62M7MS\SQLEXPRESS; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {

        }

        public AtmDbContext(string connString)
        {
            _connString = connString;

        }


        public SqlConnection OpenConnection()
        {
            _dbConnection = new SqlConnection(_connString);
            _dbConnection.Open();
            return _dbConnection;
        }

        public void CloseConnection()
        {
            if (_dbConnection?.State != System.Data.ConnectionState.Closed)
            {
                _dbConnection?.Close();
            }
        }

        bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _dbConnection.Dispose();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
