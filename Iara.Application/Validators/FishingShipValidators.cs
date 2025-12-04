using FluentValidation;
using Iara.Application.DTOs.FishingShips;

namespace Iara.Application.Validators;

public class CreateFishingShipRequestValidator : AbstractValidator<CreateFishingShipRequest>
{
    public CreateFishingShipRequestValidator()
    {
        RuleFor(x => x.IaraIdNumber)
            .NotEmpty().WithMessage("IARA ID number is required")
            .Length(10).WithMessage("IARA ID must be exactly 10 characters")
            .Matches("^[A-Z0-9]+$").WithMessage("IARA ID must contain only uppercase letters and numbers");

        RuleFor(x => x.MaritimeNumber)
            .NotEmpty().WithMessage("Maritime number is required")
            .MaximumLength(20).WithMessage("Maritime number cannot exceed 20 characters");

        RuleFor(x => x.ShipName)
            .NotEmpty().WithMessage("Ship name is required")
            .MinimumLength(2).WithMessage("Ship name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Ship name cannot exceed 100 characters");

        RuleFor(x => x.OwnerName)
            .NotEmpty().WithMessage("Owner name is required")
            .MinimumLength(2).WithMessage("Owner name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Owner name cannot exceed 100 characters");

        RuleFor(x => x.Tonnage)
            .GreaterThan(0).WithMessage("Tonnage must be greater than 0")
            .LessThanOrEqualTo(10000).WithMessage("Tonnage cannot exceed 10000 tons");

        RuleFor(x => x.ShipLength)
            .GreaterThan(0).WithMessage("Ship length must be greater than 0")
            .LessThanOrEqualTo(200).WithMessage("Ship length cannot exceed 200 meters");

        RuleFor(x => x.EnginePower)
            .GreaterThan(0).WithMessage("Engine power must be greater than 0")
            .LessThanOrEqualTo(5000).WithMessage("Engine power cannot exceed 5000 HP");

        RuleFor(x => x.FuelType)
            .NotEmpty().WithMessage("Fuel type is required")
            .Must(x => x == "Diesel" || x == "Petrol" || x == "Electric" || x == "Hybrid")
            .WithMessage("Fuel type must be Diesel, Petrol, Electric, or Hybrid");

        RuleFor(x => x.RegistrationDate)
            .NotEmpty().WithMessage("Registration date is required")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Registration date cannot be in the future");
    }
}

public class UpdateFishingShipRequestValidator : AbstractValidator<UpdateFishingShipRequest>
{
    public UpdateFishingShipRequestValidator()
    {
        RuleFor(x => x.ShipName)
            .NotEmpty().WithMessage("Ship name is required")
            .MinimumLength(2).WithMessage("Ship name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Ship name cannot exceed 100 characters");

        RuleFor(x => x.OwnerName)
            .NotEmpty().WithMessage("Owner name is required")
            .MinimumLength(2).WithMessage("Owner name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Owner name cannot exceed 100 characters");

        RuleFor(x => x.Tonnage)
            .GreaterThan(0).WithMessage("Tonnage must be greater than 0")
            .LessThanOrEqualTo(10000).WithMessage("Tonnage cannot exceed 10000 tons");

        RuleFor(x => x.ShipLength)
            .GreaterThan(0).WithMessage("Ship length must be greater than 0")
            .LessThanOrEqualTo(200).WithMessage("Ship length cannot exceed 200 meters");

        RuleFor(x => x.EnginePower)
            .GreaterThan(0).WithMessage("Engine power must be greater than 0")
            .LessThanOrEqualTo(5000).WithMessage("Engine power cannot exceed 5000 HP");

        RuleFor(x => x.FuelType)
            .NotEmpty().WithMessage("Fuel type is required")
            .Must(x => x == "Diesel" || x == "Petrol" || x == "Electric" || x == "Hybrid")
            .WithMessage("Fuel type must be Diesel, Petrol, Electric, or Hybrid");
    }
}
