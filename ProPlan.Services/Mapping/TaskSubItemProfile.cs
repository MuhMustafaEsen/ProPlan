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
    public class TaskSubItemProfile : Profile
    {
        public TaskSubItemProfile()
        {
            // TaskSubItem -> TaskSubItemDtoForRead
            CreateMap<TaskSubItem, TaskSubItemDtoForRead>()
                .ForMember(dest => dest.TaskAssignmentId,
                    opt => opt.MapFrom(src => src.TaskAssignmentId))

                .ForMember(dest => dest.TaskDate,
                    opt => opt.MapFrom(src => src.TaskAssignment.TaskDate))

                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.TaskAssignment.UserId))

                .ForMember(dest => dest.CompanyTaskId,
                    opt => opt.MapFrom(src => src.TaskAssignment.CompanyTaskId))

                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            // Create
            CreateMap<TaskSubItemDtoForCreate, TaskSubItem>()
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsCompleted,
                    opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.CompletedAt,
                    opt => opt.MapFrom(_ => (DateTime?)null));

            // Update
            CreateMap<TaskSubItemDtoForUpdate, TaskSubItem>()
                .ForMember(dest => dest.CompletedAt,
                    opt => opt.MapFrom(src =>
                        src.IsCompleted ? DateTime.UtcNow : (DateTime?)null));
        }
    }

}
