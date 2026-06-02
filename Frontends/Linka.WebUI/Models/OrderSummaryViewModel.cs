using Linka.DtoLayer.BasketDtos;

namespace Linka.WebUI.Models
{
    public class OrderSummaryViewModel
    {
        public List<BasketItemDto> BasketItems { get; set; }
            = new List<BasketItemDto>();

        public string? DiscountCode { get; set; }

        public int DiscountRate { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal DiscountedSubTotal { get; set; }

        public decimal VatRate { get; set; }

        public decimal VatAmount { get; set; }

        public decimal ShippingFee { get; set; }

        public decimal GrandTotal { get; set; }

        public bool HasDiscount =>
            DiscountRate > 0 &&
            !string.IsNullOrWhiteSpace(DiscountCode);

        public bool HasFreeShipping =>
            BasketItems.Any() &&
            ShippingFee == 0;
    }
}
