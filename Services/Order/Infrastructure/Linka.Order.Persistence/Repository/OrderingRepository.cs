using Linka.Order.Application.Interfaces;
using Linka.Order.Domain.Entities;
using Linka.Order.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Linka.Order.Persistence.Repository
{
    public class OrderingRepository
        : IOrderingRepository
    {
        private readonly OrderContext
            _orderContext;

        public OrderingRepository(
            OrderContext orderContext)
        {
            _orderContext =
                orderContext;
        }

        public List<Ordering>
            GetOrderingsByUserId(string id)
        {
            return _orderContext
                .Orderings
                .Where(x =>
                    x.UserId == id)
                .OrderByDescending(x =>
                    x.OrderDate)
                .ToList();
        }

        public async Task<int>
            CreateOrderingWithDetailsAsync(
                Ordering ordering)
        {
            /*
             * Ordering nesnesinin içindeki OrderDetails
             * navigation listesi de aynı SaveChanges işleminde
             * kaydedilir.
             */
            await _orderContext
                .Orderings
                .AddAsync(ordering);

            await _orderContext
                .SaveChangesAsync();

            return ordering.OrderingId;
        }

        public async Task<Ordering?>
    GetOrderingWithDetailsByIdAsync(
        int orderingId,
        string userId)
        {
            return await _orderContext
                .Orderings
                .Include(x =>
                    x.OrderDetails)
                .FirstOrDefaultAsync(x =>
                    x.OrderingId == orderingId &&
                    x.UserId == userId);
        }

        public async Task<bool>
    UpdateOrderingStatusAsync(
        int orderingId,
        string orderStatus)
        {
            var ordering =
                await _orderContext
                    .Orderings
                    .FirstOrDefaultAsync(x =>
                        x.OrderingId ==
                        orderingId);

            if (ordering == null)
            {
                return false;
            }

            ordering.OrderStatus =
                orderStatus;

            await _orderContext
                .SaveChangesAsync();

            return true;
        }

        public async Task<List<Ordering>>
    GetAllOrderingsAsync()
        {
            return await _orderContext
                .Orderings
                .OrderByDescending(x =>
                    x.OrderDate)
                .ToListAsync();
        }
    }
}