using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Floor;

namespace IDB.Navigator.Site.Areas.Admin.Models.Floor
{
    [Validator(typeof(EditFloorValidator))]
    public class EditFloorModel : FloorModel
    {
    }
}