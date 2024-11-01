using MediatR;

namespace ProductSalesAPI.Application.SalesOrders.Command.CreateSalesOrder;

public record CreateSalesOrderCommand(int ProductId, int CustomerId, int Quantity) : IRequest<int>;
