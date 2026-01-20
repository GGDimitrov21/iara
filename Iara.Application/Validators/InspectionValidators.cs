using FluentValidation;
using Iara.Application.DTOs.Inspections;

namespace Iara.Application.Validators;

public class CreateInspectionRequestValidator : AbstractValidator<CreateInspectionRequest>
{
    public CreateInspectionRequestValidator()
    {
        RuleFor(x => x.VesselId)
            .GreaterThan(0).WithMessage("Vessel ID must be greater than 0");

        RuleFor(x => x.InspectorId)
            .GreaterThan(0).WithMessage("Inspector ID must be greater than 0");

        RuleFor(x => x.InspectionDate)
            .NotEmpty().WithMessage("Inspection date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Inspection date cannot be in the future");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
