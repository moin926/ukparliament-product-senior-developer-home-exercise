using FluentValidation;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Validators;

public class PersonRequestValidator : AbstractValidator<PersonViewModel>
{
    public PersonRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.DateOfBirth.Date)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.Now.Date).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("A valid department must be selected.");
    }
}
