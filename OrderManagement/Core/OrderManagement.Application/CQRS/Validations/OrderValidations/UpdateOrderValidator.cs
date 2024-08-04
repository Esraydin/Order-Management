using FluentValidation;
using OrderManagement.Application.CQRS.Commands.OrderCommands;

namespace OrderManagement.Application.CQRS.Validations.OrderValidations;

public class UpdateProductCategoryValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateProductCategoryValidator()
    {
        RuleFor(p => p.Name)
            .NotNull()
            .WithMessage("Lütfen 'Name'i boş geçmeyiniz.")
            .MaximumLength(20)
            .MinimumLength(3)
            .WithMessage("'Name' değeri 3 ile 20 karakter arasında olmalıdır.");
        RuleFor(p => p.UnitPrice)
            .Must(p => p > 0)
            .WithMessage("Lütfen 'UnitPrice' değerini doğru giriniz.");
    }
}
