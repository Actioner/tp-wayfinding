using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Node;

namespace IDB.Navigator.Site.Areas.Admin.Models.Node
{
    [Validator(typeof(CreateNodeValidator))]
    public class CreateNodeModel : NodeModel
    {
    }
}