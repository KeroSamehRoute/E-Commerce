using E_Commerce.Domain.Common;

namespace E_Commerce.Domain.Entities.Products;

public class ProductBrand : BaseEntity<int>
{
    public string Name { get; set; } = default!;
}
