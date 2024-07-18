using Api.Application.Interface.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Mapper
{
    public static class Registration
    {
        public static void AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper, AutoMapper.Mapper>();
        }
    }
}
