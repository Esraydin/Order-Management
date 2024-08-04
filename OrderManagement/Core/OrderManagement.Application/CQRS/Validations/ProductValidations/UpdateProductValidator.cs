using FluentValidation;
using OrderManagement.Application.CQRS.Commands.ProductCommands;

namespace OrderManagement.Application.CQRS.Validations.ProductValidations;

public class UpdateUserValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(p => p.Name)
            .NotNull()
            .WithMessage("Lütfen 'Name'i boş geçmeyiniz.")
            .MaximumLength(20)
            .MinimumLength(3)
            .WithMessage("'Name' değeri 3 ile 20 karakter arasında olmalıdır.");
        RuleFor(p => p.Description)
            .NotNull()
            .WithMessage("Lütfen 'Description'i boş geçmeyiniz.")
            .MinimumLength(3)
            .WithMessage("'Description' değeri min 3 karakter olmalıdır.");
        RuleFor(p => p.Price)
        .Must(p => p > 0)
        .WithMessage("Lütfen 'Price' değerini doğru giriniz.");
    }
}
