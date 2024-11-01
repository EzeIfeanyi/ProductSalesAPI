using MediatR;

namespace ProductSalesAPI.Application.UserAuthentication.Command.Login;

public record LoginUserCommand(string Username, string Password) : IRequest<string>;
