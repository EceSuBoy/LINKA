using AutoMapper;
using Linka.Catalog.Dtos.ProductDtos;
using Linka.Catalog.Entities;
using Linka.Catalog.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Linka.Catalog.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Category> _categoryCollection;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database= client.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }


        public async Task CreateProductAsync(
    CreateProductDto createProductDto)
        {
            if (!createProductDto.IsFeatured)
            {
                createProductDto.FeaturedOrder =
                    0;
            }

            await ValidateFeaturedProductAsync(
                createProductDto.IsFeatured,
                createProductDto.FeaturedOrder);

            var values =
                _mapper.Map<Product>(
                    createProductDto);

            await _productCollection
                .InsertOneAsync(
                    values);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(x => x.ProductId == id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var values = await _productCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultProductDto>>(values);

        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var values = await _productCollection.Find<Product>(x => x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            var values = await _productCollection.Find(x => true).ToListAsync();
            foreach (var item in values)
            {
                item.Category = await _categoryCollection.Find(x => x.CategoryId == item.CategoryId).FirstAsync();
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string categoryId)
        {
            var values = await _productCollection.Find(x => x.CategoryId== categoryId).ToListAsync();
            foreach (var item in values)
            {
                item.Category = await _categoryCollection.Find(x => x.CategoryId == item.CategoryId).FirstAsync();
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task UpdateProductAsync(
    UpdateProductDto updateProductDto)
        {
            if (!updateProductDto.IsFeatured)
            {
                updateProductDto.FeaturedOrder =
                    0;
            }

            await ValidateFeaturedProductAsync(
                updateProductDto.IsFeatured,
                updateProductDto.FeaturedOrder,
                updateProductDto.ProductId);

            var values =
                _mapper.Map<Product>(
                    updateProductDto);

            await _productCollection
                .FindOneAndReplaceAsync(
                    x =>
                        x.ProductId ==
                        updateProductDto.ProductId,
                    values);
        }

        public async Task<
    PagedProductResultDto<
        ResultProductsWithCategoryDto>>
    GetPagedProductsWithCategoryAsync(
        string? search,
        string? categoryId,
        int page,
        int pageSize)
        {
            if (page < 1)
            {
                page =
                    1;
            }

            var allowedPageSizes =
                new[]
                {
            10,
            20,
            50
                };

            if (!allowedPageSizes
                    .Contains(pageSize))
            {
                pageSize =
                    10;
            }

            var filterBuilder =
                Builders<Product>
                    .Filter;

            var filter =
                filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(
                    search))
            {
                var safeSearch =
                    Regex.Escape(
                        search.Trim());

                filter &=
                    filterBuilder.Regex(
                        x => x.ProductName,
                        new BsonRegularExpression(
                            safeSearch,
                            "i"));
            }

            /*
             * CategoryId boş değilse yalnızca ilgili kategorinin
             * ürünlerini getiriyoruz.
             */
            if (!string.IsNullOrWhiteSpace(
                    categoryId))
            {
                filter &=
                    filterBuilder.Eq(
                        x => x.CategoryId,
                        categoryId);
            }

            /*
             * Pagination butonlarını hesaplamak için filtreye uyan
             * toplam ürün sayısını alıyoruz.
             */
            var totalCountLong =
                await _productCollection
                    .CountDocumentsAsync(
                        filter);

            var totalCount =
                Convert.ToInt32(
                    totalCountLong);

            var totalPages =
                Math.Max(
                    1,
                    (int)Math.Ceiling(
                        totalCount /
                        (double)pageSize));

            /*
             * Filtre değiştiğinde eski sayfa numarası geçersiz
             * hale gelebilir.
             *
             * Örnek:
             * Önceden sayfa 7 açıkken Apple araması yapılır ve
             * yalnızca 4 ürün kalır.
             *
             * Bu durumda kullanıcı sayfa 1'e taşınır.
             */
            if (page > totalPages)
            {
                page =
                    totalPages;
            }

            var products =
                await _productCollection
                    .Find(filter)
                    .SortBy(x =>
                        x.ProductName)
                    .Skip(
                        (page - 1) *
                        pageSize)
                    .Limit(
                        pageSize)
                    .ToListAsync();

            /*
             * Eski yöntemde her ürün için ayrı category sorgusu
             * çalışıyordu.
             *
             * Burada seçili sayfadaki kategori ID değerlerini
             * topluca alıp tek sorguyla kategorileri getiriyoruz.
             */
            var categoryIds =
                products
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(
                            x.CategoryId))
                    .Select(x =>
                        x.CategoryId)
                    .Distinct()
                    .ToList();

            var categories =
                new List<Category>();

            if (categoryIds.Any())
            {
                var categoryFilter =
                    Builders<Category>
                        .Filter
                        .In(
                            x => x.CategoryId,
                            categoryIds);

                categories =
                    await _categoryCollection
                        .Find(
                            categoryFilter)
                        .ToListAsync();
            }

            var categoryDictionary =
                categories
                    .ToDictionary(
                        x => x.CategoryId,
                        x => x);

            foreach (var product in products)
            {
                if (categoryDictionary
                        .TryGetValue(
                            product.CategoryId,
                            out var category))
                {
                    product.Category =
                        category;
                }
            }

            return new PagedProductResultDto<
                ResultProductsWithCategoryDto>
            {
                Items =
                    _mapper.Map<
                        List<
                            ResultProductsWithCategoryDto>>(
                                products),

                Page =
                    page,

                PageSize =
                    pageSize,

                TotalCount =
                    totalCount,

                TotalPages =
                    totalPages,

                Search =
                    search?.Trim() ??
                    string.Empty,

                CategoryId =
                    categoryId ??
                    string.Empty
            };
        }

        public async Task<List<ResultProductDto>>
    GetFeaturedProductsAsync()
        {
            const int featuredProductLimit =
                8;
            var featuredProducts =
                await _productCollection
                    .Find(x =>
                        x.IsFeatured)
                    .SortBy(x =>
                        x.FeaturedOrder)
                    .ThenBy(x =>
                        x.ProductName)
                    .Limit(
                        featuredProductLimit)
                    .ToListAsync();

            if (featuredProducts.Count <
                featuredProductLimit)
            {
                var selectedProductIds =
                    featuredProducts
                        .Select(x =>
                            x.ProductId)
                        .ToList();

                var remainingCount =
                    featuredProductLimit -
                    featuredProducts.Count;

                var filter =
                    Builders<Product>
                        .Filter
                        .Nin(
                            x => x.ProductId,
                            selectedProductIds);

                var fallbackProducts =
                    await _productCollection
                        .Find(filter)
                        .SortByDescending(x =>
                            x.DiscountRate)
                        .ThenBy(x =>
                            x.ProductName)
                        .Limit(
                            remainingCount)
                        .ToListAsync();

                featuredProducts
                    .AddRange(
                        fallbackProducts);
            }

            return _mapper.Map<
                List<ResultProductDto>>(
                    featuredProducts);
        }

        private async Task ValidateFeaturedProductAsync(
    bool isFeatured,
    int featuredOrder,
    string? excludedProductId = null)
        {
            if (!isFeatured)
            {
                return;
            }

            if (featuredOrder < 1 ||
                featuredOrder > 8)
            {
                throw new ArgumentException(
                    "Featured display order must be between 1 and 8.");
            }

            var filterBuilder =
                Builders<Product>.Filter;

            var countFilter =
                filterBuilder.Eq(
                    x => x.IsFeatured,
                    true);

            var orderFilter =
                filterBuilder.Eq(
                    x => x.FeaturedOrder,
                    featuredOrder);

            if (!string.IsNullOrWhiteSpace(
                    excludedProductId))
            {
                var excludeCurrentProductFilter =
                    filterBuilder.Ne(
                        x => x.ProductId,
                        excludedProductId);

                countFilter &=
                    excludeCurrentProductFilter;

                orderFilter &=
                    excludeCurrentProductFilter;
            }

            var featuredProductCount =
                await _productCollection
                    .CountDocumentsAsync(
                        countFilter);

            if (featuredProductCount >= 8)
            {
                throw new ArgumentException(
                    "A maximum of 8 products can be selected as featured.");
            }

            var orderAlreadyUsed =
                await _productCollection
                    .CountDocumentsAsync(
                        orderFilter);

            if (orderAlreadyUsed > 0)
            {
                throw new ArgumentException(
                    $"Featured display order {featuredOrder} is already assigned to another product.");
            }
        }

    }
}
