using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using IDB.Navigator.Site.Components.Validators.Device;

namespace IDB.Navigator.Site.Areas.Admin.Models.Device
{
    [Validator(typeof(EditDeviceValidator))]
    public class EditDeviceModel : DeviceModel
    {
    }
}