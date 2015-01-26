using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Node;

namespace TP.Wayfinding.Site.Components.Validators.Node
{
    public abstract class NodeValidator<T> : AbstractValidator<T>
        where T : NodeModel
    {
        protected NodeValidator()
        {
            RuleFor(x => x.FloorMapId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(0);

            RuleFor(x => x.Identifier)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedMessage(() => ValidationMessages.AlreadyExists)
                .WithLocalizedName(() => PropertyNames.Identifier);
        }
    }
}