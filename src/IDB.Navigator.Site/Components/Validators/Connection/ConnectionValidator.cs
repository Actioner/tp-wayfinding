using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Simple.Data;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Connection;

namespace IDB.Navigator.Site.Components.Validators.Connection
{
    public abstract class ConnectionValidator<T> : AbstractValidator<T>
        where T : ConnectionModel
    {
        protected ConnectionValidator()
        {
            RuleFor(x => x.NodeAId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(0);

            RuleFor(x => x.NodeBId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(0);
        }
    }
}