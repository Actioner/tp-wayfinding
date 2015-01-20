using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Connection;

namespace TP.Wayfinding.Site.Components.Validators.Connection
{
    public class CreateConnectionValidator : ConnectionValidator<CreateConnectionModel>
    {
        public CreateConnectionValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}