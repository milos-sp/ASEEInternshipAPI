using AutoMapper;
using ProductAPI.Commands;
using ProductAPI.Database.Entities;
using ProductAPI.Database.Repositories;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        IProductRepository _repository;
        IMapper _mapper;
        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        private async Task<bool> CheckIfProductExistsAsync(string productCode)
        {
            var product = await _repository.GetProductByCode(productCode);
            if (product == null)
            {
                return false;

            }
            return true;
        }

        public async Task<PagedSortedList<Product>> GetProducts(int page, int pageSize, SortOrder sortOrder, string? sortBy)
        {
            var products = await _repository.GetProducts(page, pageSize, sortOrder, sortBy);
            return _mapper.Map<PagedSortedList<Product>>(products);
        }


        public async Task<Product> CreateProduct(CreateProductCommand createProductCommand)
        {
            var checkIfProductExists = await CheckIfProductExistsAsync(createProductCommand.ProductCode);
            if (!checkIfProductExists)
            {
                var newProductEntity = _mapper.Map<ProductEntity>(createProductCommand);
                await _repository.CreateProduct(newProductEntity);
                return _mapper.Map<Product>(newProductEntity);
            }
            return null;
        }

        public async Task<Product> GetProduct(string productCode)
        {
            var productEnitity = await _repository.GetProductByCode(productCode);
            if (productEnitity == null)
            {
                return null;

            }
            return _mapper.Map<Product>(productEnitity);
        }

        public async Task<bool> DeleteProduct(string productCode)
        {
            var productEnitity = await _repository.GetProductByCode(productCode);
            if (productEnitity != null)
            {
                await _repository.DeleteProduct(productEnitity);
                return true;
            }
            return false;
        }
    }
}
