using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using TP.Wayfinding.Site.Components.Validators.Device;

namespace TP.Wayfinding.Site.Models.Device
{
    [Validator(typeof(EditDeviceValidator))]
    public class EditDeviceModel : DeviceModel
    {
    }
}