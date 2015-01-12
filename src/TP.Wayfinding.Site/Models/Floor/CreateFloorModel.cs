using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Floor;

namespace TP.Wayfinding.Site.Models.Floor
{
    [Validator(typeof(CreateFloorValidator))]
    public class CreateFloorModel : FloorModel
    {
    }
}