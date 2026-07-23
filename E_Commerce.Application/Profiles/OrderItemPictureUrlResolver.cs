using AutoMapper;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Profiles;

public class OrderItemPictureUrlResolver(IOptions<UrlSettings> options) : IValueResolver<OrderItem, OrderItemDto, string>
{

    private readonly UrlSettings _urlSettings = options.Value;

    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Product.PictureUrl))
        {
            return string.Empty;
        }

        return $"{_urlSettings.BaseUrl}{source.Product.PictureUrl}";
    }

}
