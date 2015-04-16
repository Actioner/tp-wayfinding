﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Connection;

namespace IDB.Navigator.Site.Areas.Admin.Models.Connection
{
    [Validator(typeof(EditConnectionValidator))]
    public class EditConnectionModel : ConnectionModel
    {
    }
}