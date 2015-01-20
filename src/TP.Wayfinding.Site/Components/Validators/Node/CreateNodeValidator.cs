using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Node;

namespace TP.Wayfinding.Site.Components.Validators.Node
{
    public class CreateNodeValidator : NodeValidator<CreateNodeModel>
    {
        public CreateNodeValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}