using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            var sql = """
            SELECT r.*, c.*, t.* 
            FROM restoraunt_reservations r
            JOIN restoraunt_customers c ON r.customer_id = c.customer_id
            JOIN restoraunt_tables t ON r.table_id = t.table_id
            """;
            
            return await _unitOfWork.Connection.QueryAsync<Reservation, Customer, RestorauntTable, Reservation>(
                sql,
                (reservation, customer, table) =>
                {
                    reservation.Customer = customer;
                    reservation.Table = table;
                    return reservation;
                },
                splitOn: "customer_id,table_id");
        }

        public async Task<Reservation?> GetReservationById(int id)
        {
            var sql = """
            SELECT r.*, c.*, t.* 
            FROM restoraunt_reservations r
            JOIN restoraunt_customers c ON r.customer_id = c.customer_id
            JOIN restoraunt_tables t ON r.table_id = t.table_id
            WHERE r.reservation_id = @Id
            """;
            
            var result = await _unitOfWork.Connection.QueryAsync<Reservation, Customer, RestorauntTable, Reservation>(
                sql,
                (reservation, customer, table) =>
                {
                    reservation.Customer = customer;
                    reservation.Table = table;
                    return reservation;
                },
                new { Id = id },
                splitOn: "customer_id,table_id");
                
            return result.FirstOrDefault();
        }

        public async Task CreateReservation(Reservation reservation)
        {
            var sql = """
            INSERT INTO restoraunt_reservations 
            (customer_id, table_id, reservation_date, reservation_time, guests_number, special_requests, status) 
            VALUES 
            (@CustomerId, @TableId, @ReservationDate, @ReservationTime, @GuestsNumber, @SpecialRequests, @Status)
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, reservation, _unitOfWork.Transaction);
        }

        public async Task UpdateReservation(Reservation reservation)
        {
            var sql = """
            UPDATE restoraunt_reservations SET 
            customer_id = @CustomerId,
            table_id = @TableId,
            reservation_date = @ReservationDate,
            reservation_time = @ReservationTime,
            guests_number = @GuestsNumber,
            special_requests = @SpecialRequests,
            status = @Status
            WHERE reservation_id = @ReservationId
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, reservation, _unitOfWork.Transaction);
        }

        public async Task CancelReservation(int id)
        {
            var sql = "UPDATE restoraunt_reservations SET status = 'Cancelled' WHERE reservation_id = @Id";
            await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<Reservation>> GetTodayReservations()
        {
            var sql = """
            SELECT r.*, c.*, t.* 
            FROM restoraunt_reservations r
            JOIN restoraunt_customers c ON r.customer_id = c.customer_id
            JOIN restoraunt_tables t ON r.table_id = t.table_id
            WHERE r.reservation_date = CURDATE() 
            AND r.status = 'Confirmed'
            ORDER BY r.reservation_time
            """;
            
            return await _unitOfWork.Connection.QueryAsync<Reservation, Customer, RestorauntTable, Reservation>(
                sql,
                (reservation, customer, table) =>
                {
                    reservation.Customer = customer;
                    reservation.Table = table;
                    return reservation;
                },
                splitOn: "customer_id,table_id");
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var sql = "SELECT * FROM restoraunt_customers ORDER BY last_name, first_name";
            return await _unitOfWork.Connection.QueryAsync<Customer>(sql);
        }

        public async Task<IEnumerable<RestorauntTable>> GetAllTables()
        {
            var sql = "SELECT * FROM restoraunt_tables WHERE is_available = TRUE ORDER BY table_number";
            return await _unitOfWork.Connection.QueryAsync<RestorauntTable>(sql);
        }
    }
}