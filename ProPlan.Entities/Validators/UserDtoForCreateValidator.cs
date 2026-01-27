using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class UserDtoForCreateValidator : AbstractValidator<UserDtoForCreate>
    {
        public UserDtoForCreateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad gereklidir.")
                .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad gereklidir.")
                .Length(2, 50).WithMessage("Soyad 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi gereklidir.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası gereklidir.")
                .Matches(@"^[0-9+\-\s()]+$").WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Rol gereklidir.")
                .Must(role => role == "Admin" || role == "Editor" || role == "Staff")
                .WithMessage("Rol Admin, Editor veya Staff olmalıdır.");

            RuleFor(x => x.PasswordH)
                .NotEmpty().WithMessage("Şifre gereklidir.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
        }
    }
}
