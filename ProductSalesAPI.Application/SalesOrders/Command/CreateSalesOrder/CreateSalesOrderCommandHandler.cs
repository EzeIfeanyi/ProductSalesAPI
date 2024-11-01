using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;

namespace ProductSalesAPI.Application.SalesOrders.Command.CreateSalesOrder;

public class CreateSalesOrderCommandHandler : IRequestHandler<CreateSalesOrderCommand, int>
{
    private readonly ISalesOrderService _salesOrderService;

    public CreateSalesOrderCommandHandler(ISalesOrderService salesOrderService)
    {
        _salesOrderService = salesOrderService;
    }

    public async Task<int> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        return await _salesOrderService.CreateSalesOrderAsync(request.ProductId, request.CustomerId, request.Quantity);
    }
}
