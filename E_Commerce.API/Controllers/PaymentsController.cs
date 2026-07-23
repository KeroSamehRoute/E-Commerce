using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers;

public class PaymentsController(IPaymentService paymentService, IOptions<PaymentGatewaySettings> options) : ApiBaseController
{
    private readonly IPaymentService _paymentService = paymentService;
    private readonly PaymentGatewaySettings _stripeSettings = options.Value;


    [Authorize]
    [HttpPost("{basketId}")]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId, CancellationToken cancellationToken)
    {
        return ToActionResult(await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, cancellationToken));
    }


    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _stripeSettings.WebhookSecret);

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:

                    var succeededPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (succeededPaymentIntent is not null)
                    {
                        await _paymentService.PaymentSucceeded(succeededPaymentIntent.Id);
                    }
                    break;

                case EventTypes.PaymentIntentPaymentFailed:

                    var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (failedPaymentIntent is not null)
                    {
                        await _paymentService.PaymentFailed(failedPaymentIntent.Id);
                    }
                    break;

                default:
                    break;
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            return BadRequest(ex.Message);
        }

    }

}
