using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Specifications;

internal class ProductWithTypeAndBrandSpec : BaseSpecification<Product, int>
{
    public ProductWithTypeAndBrandSpec(ProductQueryParams queryParams)
        : base
        (P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value)
            &&
                (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value)
            &&
                (string.IsNullOrWhiteSpace(queryParams.SearchValue) 
                    || P.Name.Contains(queryParams.SearchValue, StringComparison.CurrentCultureIgnoreCase))
        )
    {
        AddInclude(P => P.ProductType);

        AddInclude(P => P.ProductBrand);

        switch (queryParams.Sort)
        {
            case ProductSortingOptions.NameAsc:
                AddOrderBy(P => P.Name);
                break;

            case ProductSortingOptions.NameDesc:
                AddOrderDescBy(P => P.Name);
                break;

            case ProductSortingOptions.PriceAsc:
                AddOrderBy(P => P.Price);
                break;

            case ProductSortingOptions.PriceDesc:
                AddOrderDescBy(P => P.Price);
                break;
            default:
                AddOrderBy(P => P.Id);
                break;
        }

        ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
    }

    public ProductWithTypeAndBrandSpec(int id) : base(x => x.Id == id)
    {
        AddInclude(p => p.ProductType);

        AddInclude(p => p.ProductBrand);
    }

}
