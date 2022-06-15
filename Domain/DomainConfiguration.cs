using AutoMapper;
using Domain.Entities;
using Domain.Models.Inputs;
using Domain.Models.Outputs;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<TestEntity, TestEntityOutDto>();
            CreateMap<TestEntity, TestEntityDetails>();
            CreateMap<TestEntityInDto, TestEntity>();
        }
    }

    public static class DomainConfigurationExtensions
    {
        public static IServiceCollection AddDomainMappings(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainMappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}