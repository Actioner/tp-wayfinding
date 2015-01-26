using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Models.Device;

namespace TP.Wayfinding.Site.Components.Validators.Device
{
    public class EditDeviceValidator : DeviceValidator<EditDeviceModel>
    {
        public EditDeviceValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((int)0);
        }
    }
}