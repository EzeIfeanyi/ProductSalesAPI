using MediatR;

namespace ProductSalesAPI.Application.UserAuthentication.Command.Register;

public record RegisterUserCommand(string Username, string Password) : IRequest<int>;
