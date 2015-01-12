using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Building;

namespace TP.Wayfinding.Site.Models.Building
{
    [Validator(typeof(CreateBuildingValidator))]
    public class CreateBuildingModel : BuildingModel
    {
    }
}