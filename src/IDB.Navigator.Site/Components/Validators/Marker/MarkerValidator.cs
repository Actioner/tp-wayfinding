using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Marker;

namespace IDB.Navigator.Site.Components.Validators.Marker
{
    public abstract class MarkerValidator<T> : AbstractValidator<T>
        where T : MarkerModel
    {
        protected MarkerValidator()
        {
            RuleFor(x => x.DisplayName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Name);

            RuleFor(x => x.MarkerTypeId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .GreaterThan(0)
                .WithLocalizedName(() => PropertyNames.MarkerType);

            RuleFor(x => x.FloorMapId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .GreaterThan(0);
        }
    }
}