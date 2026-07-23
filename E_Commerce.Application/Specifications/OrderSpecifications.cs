using E_Commerce.Domain.Entities.Orders;

namespace E_Commerce.Application.Specifications;

internal class OrderSpecifications : BaseSpecification<Order, Guid>
{

    public OrderSpecifications(string email) : base(o => o.BuyerEmail == email)
    {
        AddInclude(o => o.DeliveryMethod);

        AddInclude(o => o.Items);

        AddOrderDescBy(o => o.OrderDate);
    }

    public OrderSpecifications(Guid id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
    {
        AddInclude(o => o.DeliveryMethod);

        AddInclude(o => o.Items);
    }

}
