using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Device;

namespace TP.Wayfinding.Site.Components.Validators.Device
{
    public class CreateDeviceValidator : DeviceValidator<CreateDeviceModel>
    {
        public CreateDeviceValidator()
        {
            RuleFor(x => x.Id)
                .Equal((int)0);
        }
    }
}