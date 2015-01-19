using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.OfficeType;

namespace TP.Wayfinding.Site.Components.Validators.OfficeType
{
    public class EditOfficeTypeValidator : OfficeTypeValidator<EditOfficeTypeModel>
    {
        public EditOfficeTypeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}