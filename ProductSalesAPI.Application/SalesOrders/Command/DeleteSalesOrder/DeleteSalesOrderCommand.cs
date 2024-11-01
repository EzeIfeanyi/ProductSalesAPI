using MediatR;

namespace ProductSalesAPI.Application.SalesOrders.Command.DeleteSalesOrder;

public record DeleteSalesOrderCommand(int Id) : IRequest<Unit>;
