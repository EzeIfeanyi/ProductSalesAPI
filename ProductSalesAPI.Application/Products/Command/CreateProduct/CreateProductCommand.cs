using MediatR;

namespace ProductSalesAPI.Application.Products.Command.CreateProduct;

public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;
