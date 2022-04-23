using API.Dto;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>().ForMember(x => x.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                                                    .ForMember(x => x.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                                                    .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<Address,AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>();
            CreateMap<BasketItemsDto, BasketItems>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
        }
        
    }
}