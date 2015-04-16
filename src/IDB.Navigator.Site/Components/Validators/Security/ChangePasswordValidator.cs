using System;
using FluentValidation;
using Simple.Data;
using IDB.Navigator.Resources;
using IDB.Navigator.Site.Components.Services;
using IDB.Navigator.Site.Areas.Admin.Models;
using IDB.Navigator.Site.Areas.Admin.Models.Account;

namespace IDB.Navigator.Site.Components.Validators.Security
{
    public class ChangePasswordValidator : AbstractValidator<ManageModel>
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
        public bool BeValid(ManageModel model)
        {
            var user = Database.Default.User.FindByEmail(model.UserName);
            return user != null && user.Password.Equals(_encryptionService.EncryptPassword(model.OldPassword), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}