using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;

namespace ProductSalesAPI.Application.UserAuthentication.Command.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
{
    private readonly IUserService _userService;

    public RegisterUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RegisterAsync(request.Username, request.Password);
    }
}
