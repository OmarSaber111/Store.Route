using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;
using Store.Route.Core.Dtos.Products;
using Store.Route.Core.Entities;
using Store.Route.Core.Helper;
using Store.Route.Core.Services.Contract;
using Store.Route.Core.Specifications.Products;

namespace Store.Route.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }



        [ProducesResponseType(typeof(PaginationResponse<ProductDto>),StatusCodes.Status200OK)]
        [HttpGet] //Get  BaseURL/api/products 
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec) //endpoint
        {
            var result = await _productService.GetAllProductsAsync(productSpec);

            return Ok(result); //200   
        }


		[ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
		[HttpGet("Brands")]//Get  BaseURL/api/products/Brands
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var result = await  _productService.GetAllBrandsAsync();

            return Ok(result);
        }

		[ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
		[HttpGet("Types")]//Get  BaseURL/api/products/Types
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var result = await  _productService.GetAllTypesAsync();

            return Ok(result);
        }

		[ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]//Get  BaseURL/api/products/
        public async Task<IActionResult> GetProductById(int? id) 
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var result = await _productService.GetProductById(id.Value);
             
            if (result is null) return NotFound(new ApiErrorResponse(404, $"The Product With Id : {id} not found in Db :("));

            return Ok(result);

        }


    }
}
