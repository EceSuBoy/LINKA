namespace Linka.Catalog.Services.StatisticsServices
{
    public interface IStatisticsService
    {
        int GetCategoryCount();
        int GetProductCount(); 
        int GetBrandCount();
        decimal GetProductAvgPrice(); 
    }
}
