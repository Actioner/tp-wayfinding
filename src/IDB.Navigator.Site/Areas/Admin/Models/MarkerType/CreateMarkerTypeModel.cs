using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.MarkerType;

namespace IDB.Navigator.Site.Areas.Admin.Models.MarkerType
{
    [Validator(typeof(CreateMarkerTypeValidator))]
    public class CreateMarkerTypeModel : MarkerTypeModel
    {
    }
}