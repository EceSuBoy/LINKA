using AutoMapper;
using Linka.Catalog.Dtos.CategoryDtos;
using Linka.Catalog.Entities;
using Linka.Catalog.Settings;
using MongoDB.Driver;

namespace Linka.Catalog.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;

        public CategoryService(
            IMapper mapper,
            IDatabaseSettings databaseSettings)
        {
            var client =
                new MongoClient(
                    databaseSettings.ConnectionString);

            var database =
                client.GetDatabase(
                    databaseSettings.DatabaseName);

            _categoryCollection =
                database.GetCollection<Category>(
                    databaseSettings.CategoryCollectionName);

            _productCollection =
                database.GetCollection<Product>(
                    databaseSettings.ProductCollectionName);

            _mapper = mapper;
        }

        public async Task CreateCategoryAsync(
            CreateCategoryDto createCategoryDto)
        {
            var value =
                _mapper.Map<Category>(
                    createCategoryDto);

            await _categoryCollection
                .InsertOneAsync(value);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryCollection
                .DeleteOneAsync(
                    x => x.CategoryId == id);
        }

        public async Task<List<ResultCategoryDto>>
            GetAllCategoriesAsync()
        {
            var categories =
                await _categoryCollection
                    .Find(x => true)
                    .ToListAsync();

            var values =
                _mapper.Map<List<ResultCategoryDto>>(
                    categories);

            foreach (var category in values)
            {
                category.ProductCount =
                    await _productCollection
                        .CountDocumentsAsync(
                            x =>
                                x.CategoryId ==
                                category.CategoryId);
            }

            return values;
        }

        public async Task<GetByIdCategoryDto>
            GetByIdCategoryAsync(string id)
        {
            var value =
                await _categoryCollection
                    .Find(x =>
                        x.CategoryId == id)
                    .FirstOrDefaultAsync();

            return _mapper.Map<GetByIdCategoryDto>(
                value);
        }

        public async Task UpdateCategoryAsync(
            UpdateCategoryDto updateCategoryDto)
        {
            var value =
                _mapper.Map<Category>(
                    updateCategoryDto);

            await _categoryCollection
                .FindOneAndReplaceAsync(
                    x =>
                        x.CategoryId ==
                        updateCategoryDto.CategoryId,
                    value);
        }
    }
}