using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Person;

namespace TP.Wayfinding.Site.Components.Validators.Person
{
    public abstract class PersonValidator<T> : AbstractValidator<T>
        where T : PersonModel
    {
        protected PersonValidator()
        {
            RuleFor(x => x.OfficeId)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .GreaterThan(0)
                  .WithLocalizedName(() => PropertyNames.Office);

            RuleFor(x => x.AccountName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must((x, y) => BeUnique(x))
                .WithLocalizedMessage(() => ValidationMessages.AlreadyExists)
                .WithLocalizedName(() => PropertyNames.AccountName);

            RuleFor(x => x.FirstName)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithLocalizedName(() => PropertyNames.FirstName);

            RuleFor(x => x.LastName)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithLocalizedName(() => PropertyNames.LastName);
        }

        private bool BeUnique(T model)
        {
            var db = Database.Default;
            var person = db.Person.Find(db.Person.AccountName == model.AccountName && db.Person.PersonId != model.Id);
            return person == null;
        }
    }
}