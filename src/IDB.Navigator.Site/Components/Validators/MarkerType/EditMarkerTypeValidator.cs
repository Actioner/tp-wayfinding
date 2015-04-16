using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.MarkerType;

namespace IDB.Navigator.Site.Components.Validators.MarkerType
{
    public class EditMarkerTypeValidator : MarkerTypeValidator<EditMarkerTypeModel>
    {
        public EditMarkerTypeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}