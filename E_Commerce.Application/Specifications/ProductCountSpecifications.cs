using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Specifications;

internal class ProductCountSpecifications(ProductQueryParams queryParams) : BaseSpecification<Product, int>
    (p =>
            (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId)
            &&
                (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId)
            &&
                (string.IsNullOrWhiteSpace(queryParams.SearchValue)
                    || p.Name.Contains(queryParams.SearchValue!, StringComparison.CurrentCultureIgnoreCase))
    )
{ }
