using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Areas.Admin.Models.Device;

namespace IDB.Navigator.Site.Components.Validators.Device
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