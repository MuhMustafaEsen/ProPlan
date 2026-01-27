using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class CompanyDtoForCreateValidator : AbstractValidator<CompanyDtoForCreate>
    {
        public CompanyDtoForCreateValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Şirket adı gereklidir.")
                .Length(2, 25).WithMessage("Şirket adı 2 ile 25 karakter arasında olmalıdır.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres gereklidir.")
                .Length(5, 200).WithMessage("Adres 5 ile 200 karakter arasında olmalıdır.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası gereklidir.")
                .Matches(@"^[0-9+\-\s()]+$").WithMessage("Geçerli bir telefon numarası giriniz.");
        }
    }
}
