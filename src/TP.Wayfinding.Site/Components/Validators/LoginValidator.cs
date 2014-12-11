using System;
using FluentValidation;
using Simple.Data;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Components.Services;
using TP.Wayfinding.Site.Models;

namespace TP.Wayfinding.Site.Components.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        private readonly IEncryptionService _encryptionService;
        public LoginValidator()
        {
            _encryptionService = new EncryptionService();

            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .EmailAddress()
                .Must((x, y) => BeEnabled(x))
                .WithLocalizedMessage(() => ValidationMessages.EmailDisabled)
                .WithLocalizedName(() => PropertyNames.UserName);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
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
        public bool BeEnabled(LoginViewModel model)
        {
            var user = Database.Default.User.FindByEmail(model.UserName);
            return user == null || user.Enabled;
        }

        public bool BeValidCombination(LoginViewModel model)
        {
            var user = Database.Default.User.FindByEmail(model.UserName);
            return user != null
                && model.Password != null
                && _encryptionService.EncryptPassword(model.Password).Equals(user.Password, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}