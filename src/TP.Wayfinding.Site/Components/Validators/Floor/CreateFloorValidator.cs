using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Floor;

namespace TP.Wayfinding.Site.Components.Validators.Floor
{
    public class CreateFloorValidator : FloorValidator<CreateFloorModel>
    {
        public CreateFloorValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}