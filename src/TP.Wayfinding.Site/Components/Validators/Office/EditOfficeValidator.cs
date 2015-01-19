using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Office;

namespace TP.Wayfinding.Site.Components.Validators.Office
{
    public class EditOfficeValidator : OfficeValidator<EditOfficeModel>
    {
        public EditOfficeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}