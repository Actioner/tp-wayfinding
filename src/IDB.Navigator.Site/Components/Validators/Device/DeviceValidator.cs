﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Device;

namespace IDB.Navigator.Site.Components.Validators.Device
{
    public abstract class DeviceValidator<T> : AbstractValidator<T>
        where T : DeviceModel
    {
        protected DeviceValidator()
        {
            RuleFor(x => x.FloorMapId)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .GreaterThan(0);

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedName(() => PropertyNames.Name);

            RuleFor(x => x.Longitude)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .InclusiveBetween(-180, 180)
               .WithLocalizedName(() => PropertyNames.SELongitude);

            RuleFor(x => x.Latitude)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .InclusiveBetween(-90, 90)
               .WithLocalizedName(() => PropertyNames.NWLatitude);
        }
    }
}