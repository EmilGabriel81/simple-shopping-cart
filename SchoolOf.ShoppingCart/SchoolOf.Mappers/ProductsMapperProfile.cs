using AutoMapper;
using SchoolOf.Data.Models;
using SchoolOf.Dtos;

namespace SchoolOf.Mappers
{
    public class ProductsMapperProfile : Profile
    {
        public ProductsMapperProfile()
        {
            CreateMap(typeof(Product), typeof(ProductDto));
        }
    }
}
