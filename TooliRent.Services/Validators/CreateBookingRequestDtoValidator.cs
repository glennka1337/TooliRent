using FluentValidation;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Validators
{
    public class CreateBookingRequestDtoValidator : AbstractValidator<CreateBookingRequestDto>
    {
        public CreateBookingRequestDtoValidator()
        {
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate.");
            RuleFor(x => x.ToolIds).NotEmpty().WithMessage("At least one tool must be selected.");
            RuleForEach(x => x.ToolIds).GreaterThan(0);
        }
    }
}