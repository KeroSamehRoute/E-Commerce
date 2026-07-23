using E_Commerce.Domain.Entities.Orders;

namespace E_Commerce.Application.Specifications;

internal class PaymentIntentSpec(string paymentIntentId) 
    : BaseSpecification<Order, Guid>(o => o.PaymentIntentId == paymentIntentId)
{ }
