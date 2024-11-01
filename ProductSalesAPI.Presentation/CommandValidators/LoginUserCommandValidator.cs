﻿using FluentValidation;
using ProductSalesAPI.Application.UserAuthentication.Command.Login;

namespace ProductSalesAPI.Presentation.CommandValidators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
