﻿using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Specifications.Products
{
    public class ProductSpecifications : BaseSpecifications<Product , int>
    {
        public ProductSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }

        public ProductSpecifications(ProductSpecParams productSpec) : base (
            P=>
                (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
                &&
                (!productSpec.BrandId.HasValue || productSpec.BrandId == P.BrandId)
                &&
                (!productSpec.TypeId.HasValue || productSpec.TypeId == P.TypeId)
            )
        {

            //sort by : name , priceAsc , priceDesc

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price); 
                        break;
                    case "priceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy( P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

            ApplyIncludes();


            //900
            //size = 5
            //index = 2
            ApplyPagination(productSpec.PageSize, productSpec.PageSize * (productSpec.PageIndex - 1));
        }

        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
    }
}
