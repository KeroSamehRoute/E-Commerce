using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Specifications;

internal class ProductsWithIdsSpecifications(HashSet<int> productIds)
    : BaseSpecification<Product, int>(p => productIds.Contains(p.Id))
{ }
