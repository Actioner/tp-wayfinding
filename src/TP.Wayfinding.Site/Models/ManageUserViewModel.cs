using TP.Wayfinding.Site.Components.Validators;
using FluentValidation.Attributes;

namespace TP.Wayfinding.Site.Models
{
    [Validator(typeof(ChangePasswordValidator))]
    public class ManageUserViewModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
