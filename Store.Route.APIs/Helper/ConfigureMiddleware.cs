using Store.Route.APIs.Middlewares;
using Store.Route.Repository.Data.Contexts;
using Store.Route.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace Store.Route.APIs.Helper
{
	public static class ConfigureMiddleware
	{
		public static async Task<WebApplication> ConfigureMiddlewareAsync(this WebApplication app)
		{

			//StoreDbContext context = new StoreDbContext(); // I Created This Object But I need CLR Create It Instead Of Me
			//await context.Database.MigrateAsync(); //Update-Database\

			//So I  Use This |
			//               v

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var context = services.GetRequiredService<StoreDbContext>();
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await context.Database.MigrateAsync();
				await StoreDbContextSeed.SeedAsync(context);

			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "There Are Problems During Apply Migrations !");
			}

			app.UseMiddleware<ExceptionMiddleware>(); // Configure User-Defined Middleware


			app.UseStatusCodePagesWithReExecute("/error/{0}");

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			 
			app.UseStaticFiles();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();


			return app;
		}


	}
}
