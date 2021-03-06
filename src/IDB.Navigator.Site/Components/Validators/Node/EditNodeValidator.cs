﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Node;

namespace IDB.Navigator.Site.Components.Validators.Node
{
    public class EditNodeValidator : NodeValidator<EditNodeModel>
    {
        public EditNodeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}