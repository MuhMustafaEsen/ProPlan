using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class TaskSubItemDtoForCreateValidator : AbstractValidator<TaskSubItemDtoForCreate>
    {
        public TaskSubItemDtoForCreateValidator()
        {
            RuleFor(x => x.TaskAssignmentId)
                .GreaterThan(0).WithMessage("Görev ataması ID gereklidir.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık gereklidir.")
                .Length(1, 200).WithMessage("Başlık 1 ile 200 karakter arasında olmalıdır.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Sıra numarası 0 veya daha büyük olmalıdır.");
        }
    }
}
