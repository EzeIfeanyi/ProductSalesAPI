using global::ProductSalesAPI.Application.ApiUtilities.Interfaces;
using global::ProductSalesAPI.Application.ApiUtilities.Shared;
using global::ProductSalesAPI.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ProductSalesAPI.Application.ApiUtilities.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ILogger<SalesOrderService> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public SalesOrderService(
            ISalesOrderRepository salesOrderRepository,
            ILogger<SalesOrderService> logger,
            IHubContext<NotificationsHub> hubContext)
        {
            _salesOrderRepository = salesOrderRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<int> CreateSalesOrderAsync(int productId, int customerId, int quantity)
        {
            var salesOrder = new SalesOrder(productId, customerId, quantity);
            var salesOrderId = await _salesOrderRepository.AddAsync(salesOrder);
            _logger.LogInformation("Sales order created with ID {SalesOrderId}.", salesOrderId);

            await _hubContext.Clients.All.SendAsync("ReceiveOrderCreated", new
            {
                orderId = salesOrderId,
                productId,
                quantity
            });

            return salesOrderId;
        }

        public async Task DeleteSalesOrderAsync(int salesOrderId)
        {
            await _salesOrderRepository.DeleteAsync(salesOrderId);
            _logger.LogInformation("Sales order with ID {SalesOrderId} deleted successfully.", salesOrderId);
        }

        public async Task<PagedResponse<SalesOrder>> GetPagedSalesOrdersAsync(int pageNumber, int pageSize)
        {
            var salesOrders = await _salesOrderRepository.GetPagedAsync(pageNumber, pageSize);
            var totalSalesOrders = await _salesOrderRepository.GetCountAsync();

            return new PagedResponse<SalesOrder>(salesOrders, totalSalesOrders, pageNumber, pageSize);
        }
    }
}
