using E_Commerce.API.Attributes;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

public class ProductsController(IProductService productService) : ApiBaseController
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    [RedisCache(100)]
    public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery] ProductQueryParams queryParams, CancellationToken ct)
    {
        var result = await _productService.GetAllProductsAsync(queryParams, ct);
        return ToActionResult(result);
    }


    [HttpPost]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(int id, CancellationToken ct)
    {
        var result = await _productService.GetProductByIdAsync(id, ct);
        return ToActionResult(result);
    }


    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct)
    {
        return ToActionResult(await _productService.GetAllTypesAsync(ct));
    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct)
    {
        return ToActionResult(await _productService.GetAllBrandAsync(ct));
    }

}
