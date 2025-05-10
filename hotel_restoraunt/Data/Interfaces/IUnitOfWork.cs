// hotel_restoraunt/Data/Interfaces/IUnitOfWork.cs
using System;
using System.Data;
using System.Threading.Tasks;

namespace hotel_restoraunt.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoomRepository Rooms { get; }
        IBookingRepository Bookings { get; }
        IGuestRepository Guests { get; }
        IMenuItemRepository MenuItems { get; }
        ITableRepository Tables { get; }
        IOrderRepository Orders { get; }
        
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task<int> SaveChangesAsync();
    }
}