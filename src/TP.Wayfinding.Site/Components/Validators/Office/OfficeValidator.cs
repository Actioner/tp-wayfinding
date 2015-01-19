using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Office;

namespace TP.Wayfinding.Site.Components.Validators.Office
{
    public abstract class OfficeValidator<T> : AbstractValidator<T>
        where T : OfficeModel
    {
        protected OfficeValidator()
        {
            RuleFor(x => x.DisplayName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Name);

            RuleFor(x => x.OfficeType)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .GreaterThan(0)
                .WithLocalizedName(() => PropertyNames.OfficeType);

            RuleFor(x => x.FloorMapId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .GreaterThan(0);
        }
    }
}