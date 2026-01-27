using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class TaskSubItemDtoForUpdateValidator : AbstractValidator<TaskSubItemDtoForUpdate>
    {
        public TaskSubItemDtoForUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Alt görev ID gereklidir.");

            RuleFor(x => x.Title)
                .Length(1, 200).WithMessage("Başlık 1 ile 200 karakter arasında olmalıdır.")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Sıra numarası 0 veya daha büyük olmalıdır.")
                .When(x => x.Order.HasValue);
        }
    }
}
