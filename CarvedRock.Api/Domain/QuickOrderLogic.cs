using System;
using CarvedRock.Api.ApiModels;
using CarvedRock.Api.Integrations;
using CarvedRock.Api.Interfaces;
using Microsoft.Extensions.Logging;

namespace CarvedRock.Api.Domain
{
    public class QuickOrderLogic : IQuickOrderLogic
    {
        private readonly IOrderProcessingNotification _orderProcessingNotification;
        private readonly ILogger<QuickOrderLogic> _logger;

        public QuickOrderLogic(ILogger<QuickOrderLogic> logger)
        {

            _logger = logger;
        }
        public Guid PlaceQuickOrder(QuickOrder order, int customerId)
        {                      
            _logger.LogInformation("Placing order and sending update for inventory...");
            // persist order to database or wherever
           // post "orderplaced" event to rabbitmq

            var orderId = Guid.NewGuid();

            _orderProcessingNotification.QuickOrderReceived(order, customerId, orderId);

            return orderId;
        }
    }
}
