using Contacts.Mediatr.Mediatr.Contacts.Command;
using FluentValidation;

namespace Contacts.Mediatr.Validators.Contacts;

public class CreateOrUpdateContactCommandValidator: AbstractValidator<CreateOrUpdateContactCommand>
{
    public CreateOrUpdateContactCommandValidator(bool isAdd)
    {
        When(x => isAdd, () =>
        {
            RuleFor(x => x.Id).Null().WithMessage("Id must be null when adding a new contact.");
        })
        .Otherwise(() =>
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id is required when updating an existing contact.")
                              .GreaterThan(0).WithMessage("Id must be greater than 0.");
        });

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(50).WithMessage("First Name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Last Name must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+$").WithMessage("Email must be in a valid format.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number is not valid."); // E.164 format

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.MiddleInitial)
            .MaximumLength(1).WithMessage("Middle Initial must be one character.");
    }
}