using Api.Application.Bases;
using Api.Application.Behaviors;
using Api.Application.Exceptions;
using Api.Application.Features.Products.Rules;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace Api.Application
{
    public static class Registration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddTransient<ExceptionMiddleware>();

            services.AddRulesFromAssemblyContaining(assembly, typeof(BaseRules));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddValidatorsFromAssembly(assembly);
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("tr");
            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(FluentValidationBehavior<,>)); //Mediatr isteklerini doğrulama için fluentvalidationbehavior u kullanacağımızı söylüyoruz


        }

        private static IServiceCollection AddRulesFromAssemblyContaining(this IServiceCollection services, Assembly assembly, Type type)
        {
            var types = assembly.GetTypes().Where(x => x.IsSubclassOf(type) && type != x).ToList();
            foreach (var item in types)
                services.AddTransient(item);

            return services;
        }
    }
}
