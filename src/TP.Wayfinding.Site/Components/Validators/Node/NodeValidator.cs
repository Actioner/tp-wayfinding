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
                .Must((x, y) => BeUnique(x))
                .WithLocalizedMessage(() => ValidationMessages.AlreadyExists)
                .WithLocalizedName(() => PropertyNames.Identifier);
        }


        private bool BeUnique(T model)
        {
            var db = Database.Default;
            var node = db.Node.Find(db.Node.Identifier == model.Identifier && db.Node.FloorMapId == model.FloorMapId && db.Node.NodeId != model.Id);
            return node == null;
        }
    }
}