using AutoMapper;
using OrderService.Application.Dtos;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Order , OrderDto>().ReverseMap();
        }
    }
}
