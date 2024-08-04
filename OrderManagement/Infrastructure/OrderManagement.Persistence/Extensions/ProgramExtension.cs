using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.CQRS.Handlers.CompanyHandlers;
using OrderManagement.Application.CQRS.Handlers.OrderHandlers;
using OrderManagement.Application.CQRS.Handlers.UserHandlers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Persistence.Concrete;
using OrderManagement.Persistence.Context;
using ProductCategoryManagement.Application.CQRS.Handlers.ProductCategoryHandlers;
using ProductManagement.Application.CQRS.Handlers.ProductHandlers;
using System.Reflection;

namespace OrderManagement.Persistence.Extensions
{
    public static class ProgramExtension
    {
        public static void Program(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetOrderQueryHandler).Assembly));
            services.AddScoped<IOrderRepository, OrderRepository>();

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserQueryHandler).Assembly));
            services.AddScoped<IUserRepository, UserRepository>();

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductQueryHandler).Assembly));
            services.AddScoped<IProductRepository, ProductRepository>();

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCompanyQueryHandler).Assembly));
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductCategoryQueryHandler).Assembly));
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
