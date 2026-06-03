using Linka.DtoLayer.DiscountDtos;

namespace Linka.WebUI.Models
{
    public class CouponListViewModel
    {
        public List<ResultDiscountCouponDto> Items { get; set; } =
            new List<ResultDiscountCouponDto>();

        public string Search { get; set; } =
            string.Empty;

        public string StatusFilter { get; set; } =
            "All";

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int ActiveCount { get; set; }

        public int InactiveCount { get; set; }

        public int ExpiredCount { get; set; }
    }
}