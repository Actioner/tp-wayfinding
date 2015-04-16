using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Marker;

namespace IDB.Navigator.Site.Areas.Admin.Models.Marker
{
    [Validator(typeof(EditMarkerValidator))]
    public class EditMarkerModel : MarkerModel
    {
    }
}