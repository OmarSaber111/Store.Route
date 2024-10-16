using Microsoft.EntityFrameworkCore;
using Store.Route.Core.Entities;
using Store.Route.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Repository
{
    public static class SpecificationsEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        // _context.Products.Where(P => P.Id == id as int?).Include(p => p.Brand).Include(p => p.Type).FirstOrDefaultAsync() as TEntity;
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity , TKey> spec)
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }


            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }


            if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPaginationEnabled)
            {
                query = query.Take(spec.Take).Skip(spec.Skip);
            }

            //p=>p.brand
            //p=>p.type


            // _context.Products.Where(P => P.Id == id as int?)
            // _context.Products.Where(P => P.Id == id as int?).include(p=>p.brand)
            // _context.Products.Where(P => P.Id == id as int?).include(p=>p.brand).include(p=>p.type)

            query = spec.Includes.Aggregate(query , (currentQuery , IncludeExpression) => currentQuery.Include(IncludeExpression));

            return query;
        }
    }
}
