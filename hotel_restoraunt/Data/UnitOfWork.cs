// Data/UnitOfWork.cs
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using hotel_restoraunt.Data.Interfaces;

namespace hotel_restoraunt.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IDbTransaction _transaction;
        
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            Connection = context.CreateConnection();
        }

        public IDbConnection Connection { get; }
        public IDbTransaction Transaction => _transaction;

        public void BeginTransaction()
        {
            if (_transaction == null)
                _transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}