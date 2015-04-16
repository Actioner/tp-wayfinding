using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Device;

namespace IDB.Navigator.Site.Components.Validators.Device
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