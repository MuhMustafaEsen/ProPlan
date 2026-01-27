using FluentValidation;
using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Validators
{
    public class TaskAssignmentDtoForCreateValidator : AbstractValidator<TaskAssignmentDtoForCreate>
    {
        public TaskAssignmentDtoForCreateValidator()
        {
            RuleFor(x => x.CompanyTaskId)
                .GreaterThan(0).WithMessage("Şirket görevi ID gereklidir.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Kullanıcı ID gereklidir.");

            RuleFor(x => x.TaskDate)
                .NotEmpty().WithMessage("Görev tarihi gereklidir.");
        }
    }
}
