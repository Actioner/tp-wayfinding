using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Building;

namespace TP.Wayfinding.Site.Components.Validators.Building
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