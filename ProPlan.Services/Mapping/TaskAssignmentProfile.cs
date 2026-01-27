using AutoMapper;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Mapping
{
    public class TaskAssignmentProfile : Profile
    {
        //eskisi
        /*
        public TaskAssignmentProfile()
        {
            CreateMap<TaskAssignment, TaskAssignmentDtoForRead>();
            CreateMap<TaskAssignmentDtoForCreate, TaskAssignment>();
            CreateMap<TaskAssignmentDtoForUpdate, TaskAssignment>()
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore());
        }
        */

        public TaskAssignmentProfile()
        {
            CreateMap<TaskAssignment, MonthlyTaskAssignmentDto>()
            .ForMember(
                dest => dest.CompanyId,
                opt => opt.MapFrom(src => src.CompanyTask.CompanyId)
            )
            .ForMember(
                dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.CompanyTask.Company.CompanyName)
            )
            .ForMember(
                dest => dest.TaskName,
                opt => opt.MapFrom(src => src.CompanyTask.TaskDefinition.TaskName)
            )
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.FirstName)
            )
            .ForMember(
                dest => dest.PlannedDate,
                opt => opt.MapFrom(src => src.TaskDate)
            )
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src =>
                    src.IsCompleted ? "Completed" : "Planned"
                )
            );

            CreateMap<TaskAssignment, TaskAssignmentDtoForRead>()
                .ForMember(d => d.CompanyName,
                    o => o.MapFrom(s => s.CompanyTask.Company.CompanyName))
                .ForMember(d => d.TaskName,
                    o => o.MapFrom(s => s.CompanyTask.TaskDefinition.TaskName))
                .ForMember(d => d.UserName,
                    o => o.MapFrom(s => s.User.FirstName + " " + s.User.LastName));

            CreateMap<TaskAssignmentDtoForCreate, TaskAssignment>();
            CreateMap<TaskAssignmentDtoForUpdate, TaskAssignment>();
        }
    }

}
