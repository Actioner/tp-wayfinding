using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Building;

namespace TP.Wayfinding.Site.Components.Validators.Building
{
    public class CreateBuildingValidator : BuildingValidator<CreateBuildingModel>
    {
        public CreateBuildingValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}