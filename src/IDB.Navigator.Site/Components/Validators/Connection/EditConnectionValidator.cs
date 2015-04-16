using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Connection;

namespace IDB.Navigator.Site.Components.Validators.Connection
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