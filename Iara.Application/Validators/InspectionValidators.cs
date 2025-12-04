using FluentValidation;
using Iara.Application.DTOs.Inspections;

namespace Iara.Application.Validators;

public class CreateInspectionRequestValidator : AbstractValidator<CreateInspectionRequest>
{
    public CreateInspectionRequestValidator()
    {
        RuleFor(x => x.ShipId)
            .GreaterThan(0).WithMessage("Ship ID must be greater than 0");

        RuleFor(x => x.InspectionLocation)
            .NotEmpty().WithMessage("Inspection location is required")
            .MaximumLength(200).WithMessage("Inspection location cannot exceed 200 characters");

        RuleFor(x => x.ProtocolNumber)
            .NotEmpty().WithMessage("Protocol number is required")
            .MaximumLength(50).WithMessage("Protocol number cannot exceed 50 characters")
            .Matches("^[A-Z0-9-/]+$").WithMessage("Protocol number can only contain uppercase letters, numbers, hyphens and slashes");

        RuleFor(x => x.ViolationDescription)
            .NotEmpty().WithMessage("Violation description is required when violation is detected")
            .MinimumLength(10).WithMessage("Violation description must be at least 10 characters")
            .MaximumLength(2000).WithMessage("Violation description cannot exceed 2000 characters")
            .When(x => x.HasViolation);

        RuleFor(x => x.SanctionsImposed)
            .NotEmpty().WithMessage("Sanctions must be specified when violation is detected")
            .MaximumLength(1000).WithMessage("Sanctions description cannot exceed 1000 characters")
            .When(x => x.HasViolation);

        RuleFor(x => x.ProofOfViolationUrl)
            .Must(BeAValidUrl).WithMessage("Proof of violation URL must be a valid URL")
            .When(x => !string.IsNullOrWhiteSpace(x.ProofOfViolationUrl));

        RuleFor(x => x.ViolationDescription)
            .Empty().WithMessage("Violation description should be empty when no violation exists")
            .When(x => !x.HasViolation);

        RuleFor(x => x.SanctionsImposed)
            .Empty().WithMessage("Sanctions should be empty when no violation exists")
            .When(x => !x.HasViolation);
    }

    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
