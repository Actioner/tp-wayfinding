using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Node;

namespace TP.Wayfinding.Site.Models.Node
{
    [Validator(typeof(CreateNodeValidator))]
    public class CreateNodeModel : NodeModel
    {
    }
}