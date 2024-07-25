using AutoMapper;
using ProductAPI.Commands;
using ProductAPI.Database.Entities;
using ProductAPI.Models;
using System.Globalization;
using System.Text.RegularExpressions;
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

            // mapiranja
            /*CreateMap<TransactionEntity, Transaction>()
                .ForMember(src => src.Date, e => e.MapFrom(dst => dst.ToString()));

            CreateMap<Transaction, TransactionEntity>()
                .ForMember(src => src.Date, e => e.MapFrom(dst => DateTime.ParseExact(dst.Date, "mm/dd/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<TransactionEntity, CreateTransactionCommand>()
                .ForMember(src => src.Date, e => e.MapFrom(dst => dst.ToString()));

            CreateMap<CreateTransactionCommand, TransactionEntity>()
                .ForMember(src => src.Date, e => e.MapFrom(dst => DateTime.ParseExact(dst.Date, "mm/dd/yyyy", CultureInfo.InvariantCulture)));*/

            CreateMap<TransactionEntity, Transaction>();

            CreateMap<Transaction, TransactionEntity>();

            CreateMap<CreateTransactionCommand, Transaction>();

            CreateMap<Transaction, CreateTransactionCommand>();

            CreateMap<CreateTransactionCommand, TransactionEntity>();

            CreateMap<TransactionEntity, CreateTransactionCommand>();

            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Transaction>>();

            // CreateMap<CreateTransactionCommand, Transaction>();

        }
    }
}
