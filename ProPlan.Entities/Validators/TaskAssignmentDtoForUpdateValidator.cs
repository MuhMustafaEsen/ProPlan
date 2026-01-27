using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class TaskAssignmentDtoForUpdateValidator : AbstractValidator<TaskAssignmentDtoForUpdate>
    {
        public TaskAssignmentDtoForUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Görev ataması ID gereklidir.");

            RuleFor(x => x.CompanyTaskId)
                .GreaterThan(0).WithMessage("Şirket görevi ID gereklidir.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Kullanıcı ID gereklidir.");

            RuleFor(x => x.TaskDate)
                .NotEmpty().WithMessage("Görev tarihi gereklidir.");
        }
    }
}
