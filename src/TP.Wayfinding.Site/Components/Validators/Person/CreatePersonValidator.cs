using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Person;

namespace TP.Wayfinding.Site.Components.Validators.Person
{
    public class CreatePersonValidator : PersonValidator<CreatePersonModel>
    {
        public CreatePersonValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}