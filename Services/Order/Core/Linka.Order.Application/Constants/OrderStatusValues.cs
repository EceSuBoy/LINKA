namespace Linka.Order.Application.Constants
{
    public static class OrderStatusValues
    {
        public const string Paid =
            "Paid";

        public const string Preparing =
            "Preparing";

        public const string Shipped =
            "Shipped";

        public const string Delivered =
            "Delivered";

        public const string Cancelled =
            "Cancelled";

        public static readonly IReadOnlyCollection<string>
            AllStatuses =
                new List<string>
                {
                    Paid,
                    Preparing,
                    Shipped,
                    Delivered,
                    Cancelled
                };

        public static bool IsValid(
            string? orderStatus)
        {
            if (string.IsNullOrWhiteSpace(
                    orderStatus))
            {
                return false;
            }

            return AllStatuses.Any(x =>
                string.Equals(
                    x,
                    orderStatus.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        public static string Normalize(
            string orderStatus)
        {
            return AllStatuses.First(x =>
                string.Equals(
                    x,
                    orderStatus.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }
    }
}