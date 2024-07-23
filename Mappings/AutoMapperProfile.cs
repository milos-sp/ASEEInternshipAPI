using AutoMapper;
using ProductAPI.Commands;
using ProductAPI.Database.Entities;
using ProductAPI.Models;
namespace ProductAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<ProductEntity, Product>()
                .ForMember(p => p.ProductCode, e => e.MapFrom(x => x.Code));

            CreateMap<Product, ProductEntity>()
                .ForMember(et => et.Code, p => p.MapFrom(x => x.ProductCode));

            CreateMap<PagedSortedList<ProductEntity>, PagedSortedList<Product>>();

            CreateMap<CreateProductCommand, ProductEntity>()
                .ForMember(et => et.Code, p => p.MapFrom(x => x.ProductCode));

            CreateMap<TransactionEntity, Transaction>();

            CreateMap<CreateTransactionCommand, TransactionEntity>();

        }
    }
}
