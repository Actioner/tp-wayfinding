using IDB.Navigator.Site.Components.Validators.Security;
using FluentValidation.Attributes;

namespace IDB.Navigator.Site.Areas.Admin.Models.Account
{
    [Validator(typeof(ChangePasswordValidator))]
    public class ManageModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
