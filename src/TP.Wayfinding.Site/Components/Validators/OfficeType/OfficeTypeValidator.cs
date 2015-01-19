using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.OfficeType;

namespace TP.Wayfinding.Site.Components.Validators.OfficeType
{
    public abstract class OfficeTypeValidator<T> : AbstractValidator<T>
        where T : OfficeTypeModel
    {
        protected OfficeTypeValidator()
        {
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Description);

            RuleFor(x => x.Code)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Code);
        }
    }
}