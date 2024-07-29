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
            // mapiranja

            CreateMap<TransactionEntity, CreateTransactionCommand>()
                .ForMember(dst => dst.Date, e => e.MapFrom(src => src.Date.ToString("MM/dd/yyyy")));

            CreateMap<CreateTransactionCommand, TransactionEntity>()
                 .ForMember(dst => dst.Date, e => e.MapFrom(src => System.Convert.ToDateTime(src.Date, new DateTimeFormatInfo()
                 {
                     ShortDatePattern = "MM/dd/yyyy"
                 })));

            CreateMap<TransactionEntity, Transaction>()
                .ForMember(dst => dst.Date, e => e.MapFrom(src => src.Date.ToString("MM/dd/yyyy")));

            CreateMap<Transaction, TransactionEntity>()
                 .ForMember(dst => dst.Date, e => e.MapFrom(src => System.Convert.ToDateTime(src.Date, new DateTimeFormatInfo()
                 {
                     ShortDatePattern = "MM/dd/yyyy"
                 })));

            CreateMap<CreateTransactionCommand, Transaction>().ReverseMap(); // reverse map umesto duplog mapiranja

            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Transaction>>();

            // kategorije

            CreateMap<CreateCategoryCommand, CategoryEntity>().ReverseMap();

            CreateMap<CreateCategoryCommand, Category>().ReverseMap();

            CreateMap<CategoryEntity, Category>().ReverseMap();

            // splitovi

            CreateMap<SplitEntity, Split>().ReverseMap();

            CreateMap<SplitTransactionCommand, SplitEntity>().ReverseMap();

            CreateMap<Split, SplitTransactionCommand>().ReverseMap();
        }
    }
}
