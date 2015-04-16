using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.MarkerType;

namespace IDB.Navigator.Site.Components.Validators.MarkerType
{
    public class CreateMarkerTypeValidator : MarkerTypeValidator<CreateMarkerTypeModel>
    {
        public CreateMarkerTypeValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}