using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Store.Route.APIs.Errors;
using Store.Route.Repository.Data.Contexts;

namespace Store.Route.APIs.Controllers
{

	public class BuggyController : BaseApiController
	{
		private readonly StoreDbContext _context;

		public BuggyController(StoreDbContext context)
        {
			_context = context;
		}



		[HttpGet("notfound")] // GET: /api/Buggy/notfound
		public async Task<IActionResult> GetNotFoundRequestError()
		{
			var brand = await	_context.Brands.FindAsync(100);

            if (brand is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            

			return Ok(brand);

        }



		[HttpGet("servererror")] // GET: /api/Buggy/servererror
		public async Task<IActionResult> GetServerError()
		{
			var brand = await	_context.Brands.FindAsync(100);
			var brandToString =  brand.ToString();
           
			return Ok(brandToString);

        }


		[HttpGet("badrequest")] // GET: /api/Buggy/badrequest
		public async Task<IActionResult> GetBadRequestError()
		{

			return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

        }


		[HttpGet("badrequest/{id}")] // GET: /api/Buggy/badrequest/mahmoud
		public async Task<IActionResult> GetBadRequestError(int id) //Validation Error 
		{

			return Ok();

        }

		[HttpGet("unauthorized")] // GET: /api/Buggy/unauthorized
		public async Task<IActionResult> GetUnauthorizedError() //Validation Error 
		{

			return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

        }

	}
}
