using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Route.Core.Dtos.Products;
using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductDto>()
                .ForMember(P => P.BrandName, options => options.MapFrom(s => s.Brand.Name))
                .ForMember(P => P.TypeName, options => options.MapFrom(s => s.Type.Name))
                //.ForMember(P => P.PictureUrl , options => options.MapFrom(s => $"https://localhost:7043/{s.PictureUrl}"))
                //.ForMember(P => P.PictureUrl , options => options.MapFrom(s => $"{configuration["BASEURL"]}{s.PictureUrl}"))
                .ForMember(P => P.PictureUrl, options => options.MapFrom(new PictureUrlResolver(configuration)))
                ;
            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
