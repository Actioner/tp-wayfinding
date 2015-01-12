using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Floor;

namespace TP.Wayfinding.Site.Components.Validators.Floor
{
    public class EditFloorValidator : FloorValidator<EditFloorModel>
    {
        public EditFloorValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}