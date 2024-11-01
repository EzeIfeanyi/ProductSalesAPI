using FluentValidation;
using ProductSalesAPI.Application.SalesOrders.Command.CreateSalesOrder;

namespace ProductSalesAPI.Presentation.CommandValidators;

public class CreateSalesOrderCommandValidator : AbstractValidator<CreateSalesOrderCommand>
{
    public CreateSalesOrderCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Product ID must be greater than 0.");
        RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("Customer ID must be greater than 0.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
