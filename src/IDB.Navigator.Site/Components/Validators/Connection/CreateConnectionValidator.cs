using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Connection;

namespace IDB.Navigator.Site.Components.Validators.Connection
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