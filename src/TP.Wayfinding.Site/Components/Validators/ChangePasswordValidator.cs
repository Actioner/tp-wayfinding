using System;
using FluentValidation;
using Simple.Data;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Components.Services;
using TP.Wayfinding.Site.Models;

namespace TP.Wayfinding.Site.Components.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ManageUserViewModel>
    {
        private readonly IEncryptionService _encryptionService;

        public ChangePasswordValidator()
        {
            _encryptionService = new EncryptionService();

            RuleFor(x => x.OldPassword)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .Must((x, y) => BeValid(x))
               .WithLocalizedMessage(() => ValidationMessages.InvalidProperty)
               .WithLocalizedName(() => PropertyNames.OldPassword);

            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(6, 256)
                .WithLocalizedName(() => PropertyNames.NewPassword);

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Equal(x => x.NewPassword)
                .WithLocalizedMessage(() => ValidationMessages.PasswordsDoNotMatch)
                .WithLocalizedName(() => PropertyNames.ConfirmPassword);
        }

        /// <summary>
        /// Revisa que el usuario tenga el mail que se queire cambiar
        /// </summary>
        /// <param name="model">El comando enviado por el usuario para cambiar datos.</param>
        /// <returns>Devuelve true si el password del usuario coincide con el guardado.</returns>
        public bool BeValid(ManageUserViewModel model)
        {
            var user = Database.Default.User.FindByEmail(model.UserName);
            return user != null && user.Password.Equals(_encryptionService.EncryptPassword(model.OldPassword), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}