using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Person;

namespace TP.Wayfinding.Site.Components.Validators.Person
{
    public class EditPersonValidator : PersonValidator<EditPersonModel>
    {
        public EditPersonValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}