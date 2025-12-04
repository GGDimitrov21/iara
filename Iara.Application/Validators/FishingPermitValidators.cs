using FluentValidation;
using Iara.Application.DTOs.FishingPermits;

namespace Iara.Application.Validators;

public class CreateFishingPermitRequestValidator : AbstractValidator<CreateFishingPermitRequest>
{
    public CreateFishingPermitRequestValidator()
    {
        RuleFor(x => x.ShipId)
            .GreaterThan(0).WithMessage("Ship ID must be greater than 0");

        RuleFor(x => x.PermitYear)
            .GreaterThanOrEqualTo(2000).WithMessage("Permit year must be at least 2000")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage("Permit year cannot be more than 1 year in the future");

        RuleFor(x => x.ValidFrom)
            .NotEmpty().WithMessage("Valid from date is required");

        RuleFor(x => x.ValidUntil)
            .NotEmpty().WithMessage("Valid until date is required")
            .GreaterThan(x => x.ValidFrom).WithMessage("Valid until must be after valid from date");

        RuleFor(x => x.CatchQuotaType)
            .NotEmpty().WithMessage("Catch quota type is required")
            .MaximumLength(50).WithMessage("Catch quota type cannot exceed 50 characters");

        RuleFor(x => x.MinAnnualCatch)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum annual catch cannot be negative")
            .When(x => x.MinAnnualCatch.HasValue);

        RuleFor(x => x.MaxAnnualCatch)
            .GreaterThan(0).WithMessage("Maximum annual catch must be greater than 0")
            .GreaterThan(x => x.MinAnnualCatch ?? 0).WithMessage("Maximum catch must be greater than minimum catch")
            .When(x => x.MaxAnnualCatch.HasValue);

        RuleFor(x => x.TotalHoursAnnualLimit)
            .GreaterThan(0).WithMessage("Total hours annual limit must be greater than 0")
            .LessThanOrEqualTo(8760).WithMessage("Total hours cannot exceed 8760 (365 days)")
            .When(x => x.TotalHoursAnnualLimit.HasValue);
    }
}
