﻿using Store.Route.APIs.Errors;
using System.Text.Json;

namespace Store.Route.APIs.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger ,IHostEnvironment env)
        {
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{

				_logger.LogError(ex, ex.Message);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				var response = _env.IsDevelopment() ?
					new ApiExceptionResponse(StatusCodes.Status500InternalServerError, ex.Message, ex.StackTrace?.ToString())
					: new ApiExceptionResponse(StatusCodes.Status500InternalServerError);

				var json = JsonSerializer.Serialize(response);
				await context.Response.WriteAsync(json);
			}
		}
    }
}
