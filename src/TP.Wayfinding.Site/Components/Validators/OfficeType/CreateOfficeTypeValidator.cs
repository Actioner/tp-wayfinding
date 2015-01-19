using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.OfficeType;

namespace TP.Wayfinding.Site.Components.Validators.OfficeType
{
    public class CreateOfficeTypeValidator : OfficeTypeValidator<CreateOfficeTypeModel>
    {
        public CreateOfficeTypeValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}