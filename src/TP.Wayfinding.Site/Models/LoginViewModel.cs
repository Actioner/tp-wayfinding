using TP.Wayfinding.Site.Components.Validators;
using FluentValidation.Attributes;

namespace TP.Wayfinding.Site.Models
{
    [Validator(typeof(LoginValidator))]
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}