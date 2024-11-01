using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;

namespace ProductSalesAPI.Application.SalesOrders.Command.DeleteSalesOrder;

public class DeleteSalesOrderCommandHandler : IRequestHandler<DeleteSalesOrderCommand, Unit>
{
    private readonly ISalesOrderService _salesOrderService;

    public DeleteSalesOrderCommandHandler(ISalesOrderService salesOrderService)
    {
        _salesOrderService = salesOrderService;
    }

    public async Task<Unit> Handle(DeleteSalesOrderCommand request, CancellationToken cancellationToken)
    {
        await _salesOrderService.DeleteSalesOrderAsync(request.Id);
        return Unit.Value;
    }
}
