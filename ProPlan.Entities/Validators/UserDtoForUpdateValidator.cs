using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class UserDtoForUpdateValidator : AbstractValidator<UserDtoForUpdate>
    {
        public UserDtoForUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Kullanıcı ID gereklidir.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad gereklidir.")
                .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad gereklidir.")
                .Length(2, 50).WithMessage("Soyad 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası gereklidir.")
                .Matches(@"^[0-9+\-\s()]+$").WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Rol gereklidir.")
                .Must(role => role == "Admin" || role == "Editor" || role == "Staff")
                .WithMessage("Rol Admin, Editor veya Staff olmalıdır.");
        }
    }
}
