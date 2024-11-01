using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Application.UserAuthentication.Command.Login;
using ProductSalesAPI.Application.UserAuthentication.Command.Register;

namespace ProductSalesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<int>>> Register([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        var user = new { id = userId, username = command.Username };
        _logger.LogInformation("User registered successfully with ID {UserId}.", userId);
        return CreatedAtAction(nameof(Register), new { id = userId }, new ApiResponse<object>(true, "User registered successfully.", user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            var token = await _mediator.Send(command);
            _logger.LogInformation("User {Username} logged in successfully.", command.Username);
            return Ok(new ApiResponse<string>(true, "Login successful.", token));
        }
        catch (Exception)
        {
            return BadRequest(new ApiResponse<string>(false, "Invalid username or password.", null));
        }
    }
}
