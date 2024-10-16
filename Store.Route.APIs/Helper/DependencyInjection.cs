using Microsoft.EntityFrameworkCore;
using Store.Route.Core.Services.Contract;
using Store.Route.Core;
using Store.Route.Repository;
using Store.Route.Repository.Data.Contexts;
using Store.Route.Service.Services.Products;
using Store.Route.Core.Mapping.Products;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;
using Store.Route.Core.Repositories.Contract;
using Store.Route.Repository.Repositories;
using StackExchange.Redis;
using Store.Route.Core.Mapping.Baskets;

namespace Store.Route.APIs.Helper
{
	public static class DependencyInjection
	{

		public static IServiceCollection AddDependency(this IServiceCollection services , IConfiguration configuration)
		{
			services.AddBuiltInService();
			services.AddSwaggerService();
			services.AddDbContextService(configuration);
			services.AddUserDefinedService();
			services.AddAutoMapperService(configuration);
			services.ConfigureInvalidModelStateResponseService();
			services.AddRedisService(configuration);



			return services;
		}

		private static IServiceCollection AddBuiltInService(this IServiceCollection services) 
		{
			services.AddControllers();
			return services;
		}

		private static IServiceCollection AddSwaggerService(this IServiceCollection services) 
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			return services;
		}

		private static IServiceCollection AddDbContextService(this IServiceCollection services , IConfiguration configuration) 
		{
			services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Default"));
			});

			return services;
		}

		private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
		{
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IBasketRepository, BasketRepository>();

			return services;
		}

		private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
			services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));


			return services;
		}

		private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
		{

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = actionContext =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
														  .SelectMany(P => P.Value.Errors)
														  .Select(E => E.ErrorMessage)
														  .ToArray();

					var response = new ApiValidationErrorResponse()
					{
						Errors = errors,
					};

					return new BadRequestObjectResult(response);
				};
			});


			return services;
		}
		
		private static IServiceCollection AddRedisService(this IServiceCollection services , IConfiguration configuration)
		{

			services.AddSingleton<IConnectionMultiplexer>((serviceProvider)=>
			{
				var connect = configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connect);
			});

			return services;
		}
	}
}
