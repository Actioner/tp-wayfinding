using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.OfficeType;

namespace TP.Wayfinding.Site.Models.OfficeType
{
    [Validator(typeof(CreateOfficeTypeValidator))]
    public class CreateOfficeTypeModel : OfficeTypeModel
    {
    }
}