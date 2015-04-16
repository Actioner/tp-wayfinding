using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Building;

namespace IDB.Navigator.Site.Areas.Admin.Models.Building
{
    [Validator(typeof(EditBuildingValidator))]
    public class EditBuildingModel : BuildingModel
    {
    }
}