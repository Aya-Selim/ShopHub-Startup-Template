using AutoMapper;
using myshop.Business.DTOs;
using myshop.Entities.Models;

namespace myshop.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category Maps
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Product Maps
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
