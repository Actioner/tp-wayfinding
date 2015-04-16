using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Floor;

namespace IDB.Navigator.Site.Components.Validators.Floor
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