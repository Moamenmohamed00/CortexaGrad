using FluentValidation;

namespace Cortexa.Application.Features.Patients.Commands
{
    public class AdmitPatientCommandValidator : AbstractValidator<AdmitPatientCommand>
    {
        public AdmitPatientCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.NationalId)
                .NotEmpty()
                .Length(14).WithMessage("National ID must be 14 digits.")
                .Matches(@"^\d{14}$").WithMessage("National ID must contain only digits.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.UtcNow.AddYears(-1))
                .WithMessage("Patient must be at least 1 year old.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(100).WithMessage("State must not exceed 100 characters.");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Admitting doctor ID is required.");

            RuleFor(x => x.InitialDiagnosis)
                .NotEmpty().WithMessage("Initial diagnosis is required.")
                .MaximumLength(1000).WithMessage("Initial diagnosis must not exceed 1000 characters.");

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("A valid email address is required.");
            });

            When(x => !string.IsNullOrEmpty(x.ZipCode), () =>
            {
                RuleFor(x => x.ZipCode)
                    .MaximumLength(20).WithMessage("Zip code must not exceed 20 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.Phone), () =>
            {
                RuleFor(x => x.Phone)
                    .Matches(@"^\+?\d{10,15}$")
                    .WithMessage("Invalid phone number.");
            });
        }
    }
}
