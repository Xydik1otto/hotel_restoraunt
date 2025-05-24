using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class GuestService : IGuestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GuestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Guest>> GetAllGuests()
        {
            var sql = "SELECT * FROM hotel_guests ORDER BY last_name, first_name";
            return await _unitOfWork.Connection.QueryAsync<Guest>(sql);
        }

        public async Task<Guest?> GetGuestById(int id)
        {
            var sql = "SELECT * FROM hotel_guests WHERE guest_id = @Id";
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Guest>(sql, new { Id = id });
        }

        public async Task<Guest?> GetGuestByPassport(string passportNumber)
        {
            var sql = "SELECT * FROM hotel_guests WHERE passport_number = @PassportNumber";
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Guest>(sql, new { PassportNumber = passportNumber });
        }

        public async Task AddGuest(Guest guest)
        {
            var sql = """
            INSERT INTO hotel_guests 
            (first_name, last_name, passport_number, phone_number, email, registration_date) 
            VALUES 
            (@FirstName, @LastName, @PassportNumber, @PhoneNumber, @Email, @RegistrationDate)
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, guest, _unitOfWork.Transaction);
        }

        public async Task UpdateGuest(Guest guest)
        {
            var sql = """
            UPDATE hotel_guests SET 
            first_name = @FirstName,
            last_name = @LastName,
            passport_number = @PassportNumber,
            phone_number = @PhoneNumber,
            email = @Email
            WHERE guest_id = @GuestId
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, guest, _unitOfWork.Transaction);
        }

        public async Task DeleteGuest(int id)
        {
            var sql = "DELETE FROM hotel_guests WHERE guest_id = @Id";
            await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction);
        }
    }
}