using Dapper;
using Linka.Discount.Context;
using Linka.Discount.Dtos;

namespace Linka.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly DapperContext _context;

        public DiscountService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateCouponAsync(CreateCouponDto createCouponDto)
        {
            string query = "INSERT INTO Coupons (Code, Rate, IsActive, ValidDate) VALUES (@Code, @Rate, @IsActive, @ValidDate)";
            var parameters = new DynamicParameters();
            parameters.Add("@code", createCouponDto.Code);
            parameters.Add("@rate", createCouponDto.Rate);
            parameters.Add("@isActive", createCouponDto.IsActive);
            parameters.Add("@validDate", createCouponDto.ValidDate);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            } 
        }

        public Task DeleteCouponAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultCouponDto>> GetAllCouponAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetByIdCouponDto> GetByIdCouponAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCouponAsync(CreateCouponDto createCouponDto)
        {
            throw new NotImplementedException();
        }
    }
}
