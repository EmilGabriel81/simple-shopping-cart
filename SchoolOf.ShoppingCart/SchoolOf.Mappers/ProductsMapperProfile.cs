using AutoMapper;
using SchoolOf.Data.Models;
using SchoolOf.Dtos;

namespace SchoolOf.Mappers
{
    public class ProductsMapperProfile : Profile
    {
        public ProductsMapperProfile()
        {
            // (Source) -> (DestinationToBeMapped)
            CreateMap(typeof(Product), typeof(ProductDto));
            CreateMap(typeof(Order), typeof(OrderDto));
        }
    }
}
