using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.MarkerType;

namespace IDB.Navigator.Site.Components.Validators.MarkerType
{
    public abstract class MarkerTypeValidator<T> : AbstractValidator<T>
        where T : MarkerTypeModel
    {
        protected MarkerTypeValidator()
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