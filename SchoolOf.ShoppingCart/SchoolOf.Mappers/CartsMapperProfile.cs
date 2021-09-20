using AutoMapper;
using SchoolOf.Data.Models;
using SchoolOf.Dtos;


namespace SchoolOf.Mappers
{
    public class CartsMapperProfile: Profile
    {

        public CartsMapperProfile()
        {
            CreateMap<Cart, CartDto>()
                .ForMember(destination => destination.Status, x => x.MapFrom(source => source.Status.ToString()));
            //We map from Enum to string
        }
    }
}
