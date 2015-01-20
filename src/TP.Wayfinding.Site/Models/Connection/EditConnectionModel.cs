using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Connection;

namespace TP.Wayfinding.Site.Models.Connection
{
    [Validator(typeof(EditConnectionValidator))]
    public class EditConnectionModel : ConnectionModel
    {
    }
}