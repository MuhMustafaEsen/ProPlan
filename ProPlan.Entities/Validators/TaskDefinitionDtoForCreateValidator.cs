using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class TaskDefinitionDtoForCreateValidator : AbstractValidator<TaskDefinitionDtoForCreate>
    {
        public TaskDefinitionDtoForCreateValidator()
        {
            RuleFor(x => x.TaskName)
                .NotEmpty().WithMessage("Görev adı gereklidir.")
                .Length(3, 200).WithMessage("Görev adı 3 ile 200 karakter arasında olmalıdır.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
