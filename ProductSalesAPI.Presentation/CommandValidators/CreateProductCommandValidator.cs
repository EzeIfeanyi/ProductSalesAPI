using FluentValidation;
using ProductSalesAPI.Application.Products.Command.CreateProduct;

namespace ProductSalesAPI.Presentation.CommandValidators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}
