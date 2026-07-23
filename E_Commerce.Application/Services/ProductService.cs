using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Services;

internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandAsync(CancellationToken ct = default)
    {
        var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);

        var data = _mapper.Map<IReadOnlyList<BrandDto>>(brands);

        return Result<IReadOnlyList<BrandDto>>.Ok(data);
    }


    public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
    {
        var spec = new ProductWithTypeAndBrandSpec(queryParams);

        var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec, ct);

        var totalCount = await _unitOfWork.GetRepository<Product, int>().CountAsync(new ProductCountSpecifications(queryParams), ct);

        var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);

        return new PaginatedResult<ProductDto>(queryParams.PageIndex, queryParams.PageSize, totalCount, data);
    }


    public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
    {
        var Types = _mapper.Map<IReadOnlyList<TypeDto>>(await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct));

        return Result<IReadOnlyList<TypeDto>>.Ok(Types);
    }


    public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
    {
        var spec = new ProductWithTypeAndBrandSpec(id);

        var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec, ct);

        if (product == null)
        {
            return Error.NotFound("Product.NotFound", $"Product With Id {id} Is Not Found");
        }

        return _mapper.Map<ProductDto>(product);
    }

}
