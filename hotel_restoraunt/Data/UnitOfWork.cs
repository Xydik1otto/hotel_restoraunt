
using System.Data;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Data.Repositories;
namespace hotel_restoraunt.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IDbTransaction _transaction;
        // Репозиторії
        private IUserRepository _userRepository;
        private IRoomRepository _roomRepository;
        private IBookingRepository _bookingRepository;
        private IGuestRepository _guestRepository;
        private IMenuItemRepository _menuItemRepository;
        private ITableRepository _tableRepository;
        private IOrderRepository _orderRepository;
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            Connection = context.CreateConnection();
        }
        // Реалізація властивостей інтерфейсу IUnitOfWork
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IRoomRepository Rooms => _roomRepository ??= new RoomRepository(_context);
        public IBookingRepository Bookings => _bookingRepository ??= new BookingRepository(_context);
        public IGuestRepository Guests => _guestRepository ??= new GuestRepository(_context);
        public IMenuItemRepository MenuItems => _menuItemRepository ??= new MenuItemRepository(_context);
        public ITableRepository Tables => _tableRepository ??= new TableRepository(_context);
        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                // Реалізація збереження змін
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Обробка помилок
                throw;
            }
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