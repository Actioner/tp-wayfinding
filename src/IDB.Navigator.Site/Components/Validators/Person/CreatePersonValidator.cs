using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Person;

namespace IDB.Navigator.Site.Components.Validators.Person
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