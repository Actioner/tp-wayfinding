using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Building;

namespace IDB.Navigator.Site.Components.Validators.Building
{
    public abstract class BuildingValidator<T> : AbstractValidator<T>
        where T : BuildingModel
    {
        protected BuildingValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Name);

            RuleFor(x => x.Company)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Company);

            RuleFor(x => x.Address)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithLocalizedName(() => PropertyNames.Address);

            RuleFor(x => x.NwLongitude)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .InclusiveBetween(-180, 180)
               .WithLocalizedName(() => PropertyNames.NWLongitude);

            RuleFor(x => x.SeLongitude)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .InclusiveBetween(-180, 180)
               .WithLocalizedName(() => PropertyNames.SELongitude);

            RuleFor(x => x.NwLatitude)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .InclusiveBetween(-90, 90)
               .WithLocalizedName(() => PropertyNames.NWLatitude);

            RuleFor(x => x.SeLatitude)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .InclusiveBetween(-90, 90)
               .WithLocalizedName(() => PropertyNames.SELatitude);
        }
    }
}