// Data/Interfaces/IUnitOfWork.cs
using System.Data;

namespace hotel_restoraunt.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}