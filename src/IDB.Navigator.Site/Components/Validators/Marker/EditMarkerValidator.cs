using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Marker;

namespace IDB.Navigator.Site.Components.Validators.Marker
{
    public class EditMarkerValidator : MarkerValidator<EditMarkerModel>
    {
        public EditMarkerValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}