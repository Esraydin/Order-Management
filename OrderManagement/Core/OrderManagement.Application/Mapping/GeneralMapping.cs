using AutoMapper;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.CQRS.Commands.ProductCategoryCommands;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.CQRS.Commands.UserCommands;
using OrderManagement.Application.CQRS.Queries.CompanyQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.Application.CQRS.Results.ProductCategoryResults;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.Application.CQRS.Results.UserResults;
using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Company, CreateCompanyCommand>().ReverseMap();
            CreateMap<Company, GetCompanyByIdQueryResult>().ReverseMap();
            CreateMap<Company, GetCompanyQueryResult>().ReverseMap();
            CreateMap<Company, UpdateCompanyCommand>().ReverseMap();

            CreateMap<Order, CreateOrderCommand>().ReverseMap();
            CreateMap<Order, GetOrderByIdQueryResult>().ReverseMap();
            CreateMap<Order, GetOrderQueryResult>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();

            CreateMap<ProductCategory, CreateProductCategoryCommand>().ReverseMap();
            CreateMap<ProductCategory, GetProductCategoryByIdQueryResult>().ReverseMap();
            CreateMap<ProductCategory, GetProductCategoryQueryResult>().ReverseMap();
            CreateMap<ProductCategory, UpdateProductCategoryCommand>().ReverseMap();

            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, GetProductByIdQueryResult>().ReverseMap();
            CreateMap<Product, GetProductQueryResult>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();

            CreateMap<User, CreateUserCommand>().ReverseMap().PreserveReferences();
            CreateMap<User, GetUserByIdQueryResult>().ReverseMap();
            CreateMap<User, GetUserQueryResult>().ReverseMap();
            CreateMap<User, UpdateUserCommand>().ReverseMap();
        }
    }
}
