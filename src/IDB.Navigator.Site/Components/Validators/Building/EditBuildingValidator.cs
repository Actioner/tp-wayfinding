using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Building;

namespace IDB.Navigator.Site.Components.Validators.Building
{
    public class EditBuildingValidator : BuildingValidator<EditBuildingModel>
    {
        public EditBuildingValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}