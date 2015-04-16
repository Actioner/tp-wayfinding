using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Marker;

namespace IDB.Navigator.Site.Components.Validators.Marker
{
    public class CreateMarkerValidator : MarkerValidator<CreateMarkerModel>
    {
        public CreateMarkerValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}