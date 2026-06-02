using Linka.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Order.Application.Interfaces
{
    public interface IOrderingRepository
    {
        List<Ordering>
            GetOrderingsByUserId(string id);

        Task<int>
            CreateOrderingWithDetailsAsync(
                Ordering ordering);

        Task<Ordering?>
    GetOrderingWithDetailsByIdAsync(
        int orderingId,
        string userId);

        Task<bool> UpdateOrderingStatusAsync(
    int orderingId,
    string orderStatus);

        Task<List<Ordering>>
    GetAllOrderingsAsync();
    }
}
