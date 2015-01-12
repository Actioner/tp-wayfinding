using TP.Wayfinding.Site.Components.Validators.Security;
using FluentValidation.Attributes;

namespace TP.Wayfinding.Site.Models.Account
{
    [Validator(typeof(LoginValidator))]
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}