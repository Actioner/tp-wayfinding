using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Connection;

namespace TP.Wayfinding.Site.Components.Validators.Connection
{
    public class EditConnectionValidator : ConnectionValidator<EditConnectionModel>
    {
        public EditConnectionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}