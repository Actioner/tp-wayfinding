using System;
using FluentValidation;
using Simple.Data;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Components.Services;
using IDB.Navigator.Site.Areas.Admin.Models;
using IDB.Navigator.Site.Areas.Admin.Models.Account;

namespace IDB.Navigator.Site.Components.Validators.Security
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        private readonly IEncryptionService _encryptionService;
        public LoginValidator()
        {
            _encryptionService = new EncryptionService();

            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress()
                .Must((x, y) => BeEnabled(x))
                .WithLocalizedMessage(() => ValidationMessages.EmailDisabled)
                .WithLocalizedName(() => PropertyNames.UserName);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must((x, y) => BeValidCombination(x))
                .WithLocalizedMessage(() => ValidationMessages.InvalidUsernameOrPassword)
                .WithLocalizedName(() => PropertyNames.Password);
        }

        /// <summary>
        /// Revisa que el usuario esté habilitado
        /// </summary>
        /// <param name="model">El comando enviado por el usuario para loguearse.</param>
        /// <returns>Devuelve true si el usuario está habilitado. Si el usuario no existiera también devolverá true, para evitar
        /// confirmar a un usuario malicioso si el nombre de usuario ingresado existe o no.</returns>
        public bool BeEnabled(LoginModel model)
        {
            var user = Database.Default.User.FindByEmail(model.UserName);
            return user == null || user.Enabled;
        }

        public bool BeValidCombination(LoginModel model)
        {
            var user = Database.Default.User.FindByEmail(model.UserName);
            return user != null
                && model.Password != null
                && _encryptionService.EncryptPassword(model.Password).Equals(user.Password, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}