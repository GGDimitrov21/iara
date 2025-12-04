using FluentValidation;
using Iara.Application.DTOs.FishingLogs;

namespace Iara.Application.Validators;

public class CreateFishingLogEntryRequestValidator : AbstractValidator<CreateFishingLogEntryRequest>
{
    public CreateFishingLogEntryRequestValidator()
    {
        RuleFor(x => x.ShipId)
            .GreaterThan(0).WithMessage("Ship ID must be greater than 0");

        RuleFor(x => x.LogDate)
            .NotEmpty().WithMessage("Log date is required")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Log date cannot be in the future");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

        RuleFor(x => x.FishingZone)
            .NotEmpty().WithMessage("Fishing zone is required")
            .MaximumLength(50).WithMessage("Fishing zone cannot exceed 50 characters");

        RuleFor(x => x.RouteDetails)
            .MaximumLength(500).WithMessage("Route details cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.RouteDetails));

        RuleFor(x => x.CatchCompositions)
            .NotEmpty().WithMessage("At least one catch composition is required")
            .Must(x => x.Count <= 50).WithMessage("Cannot have more than 50 catch compositions");

        RuleForEach(x => x.CatchCompositions)
            .SetValidator(new CreateCatchCompositionRequestValidator());
    }
}

public class CreateCatchCompositionRequestValidator : AbstractValidator<CreateCatchCompositionRequest>
{
    public CreateCatchCompositionRequestValidator()
    {
        RuleFor(x => x.FishSpecies)
            .NotEmpty().WithMessage("Fish species is required")
            .MaximumLength(100).WithMessage("Fish species name cannot exceed 100 characters");

        RuleFor(x => x.WeightKg)
            .GreaterThan(0).WithMessage("Weight must be greater than 0")
            .LessThanOrEqualTo(100000).WithMessage("Weight cannot exceed 100000 kg");

        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Count cannot be negative")
            .When(x => x.Count.HasValue);

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(x => x == "Kept" || x == "Discarded" || x == "Released")
            .WithMessage("Status must be Kept, Discarded, or Released");
    }
}
