using FluentValidation;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Validators
{
    public class CreateToolDtoValidator : AbstractValidator<CreateToolDto>
    {
        public CreateToolDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);
        }
    }
}