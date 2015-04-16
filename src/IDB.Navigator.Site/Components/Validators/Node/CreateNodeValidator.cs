using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Node;

namespace IDB.Navigator.Site.Components.Validators.Node
{
    public class CreateNodeValidator : NodeValidator<CreateNodeModel>
    {
        public CreateNodeValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}