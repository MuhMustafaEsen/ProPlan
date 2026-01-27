using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class CompanyTaskDtoForCreateValidator : AbstractValidator<CompanyTaskDtoForCreate>
    {
        public CompanyTaskDtoForCreateValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Şirket ID gereklidir.");

            RuleFor(x => x.TaskDefinitionId)
                .GreaterThan(0).WithMessage("Görev tanımı ID gereklidir.");

            RuleFor(x => x.Year)
                .InclusiveBetween(2000, 2100).WithMessage("Yıl 2000 ile 2100 arasında olmalıdır.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).WithMessage("Ay 1 ile 12 arasında olmalıdır.");
        }
    }
}
