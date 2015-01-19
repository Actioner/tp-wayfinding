using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Office;

namespace TP.Wayfinding.Site.Models.Office
{
    [Validator(typeof(EditOfficeValidator))]
    public class EditOfficeModel : OfficeModel
    {
    }
}