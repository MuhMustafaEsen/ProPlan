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
    public class CompanyTaskProfile : Profile
    {
        public CompanyTaskProfile()
        {
            CreateMap<CompanyTask, CompanyTaskDtoForRead>()
                .ForMember(d => d.CompanyName,
                o => o.MapFrom(s => s.Company.CompanyName))
            .ForMember(d => d.TaskName,
                o => o.MapFrom(s => s.TaskDefinition.TaskName)); ;
            CreateMap<CompanyTaskDtoForCreate, CompanyTask>();
            CreateMap<CompanyTaskDtoForUpdate, CompanyTask>();
        }
    }
}
