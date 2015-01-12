using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Floor;

namespace TP.Wayfinding.Site.Components.Validators.Floor
{
    public abstract class FloorValidator<T> : AbstractValidator<T>
        where T : FloorModel
    {
        protected FloorValidator()
        {
            RuleFor(x => x.Floor)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must((x, y) => BeUnique(x))
                .WithLocalizedMessage(() => ValidationMessages.AlreadyExists)
                .WithLocalizedName(() => PropertyNames.Floor);

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Description);

            RuleFor(x => x.BuildingId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .GreaterThan(0);
        }

        private bool BeUnique(T model)
        {
            var db = Database.Default;
            var floor = db.FloorMap.Find(db.FloorMap.Floor == model.Floor && db.FloorMap.BuildingId == model.BuildingId);
            return floor == null;
        }
    }
}