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
    public class TaskDefinitionProfile : Profile
    {
        public TaskDefinitionProfile()
        {
            CreateMap<TaskDefinition, TaskDefinitionDtoForRead>();
            CreateMap<TaskDefinitionDtoForCreate, TaskDefinition>();
            CreateMap<TaskDefinitionDtoForUpdate, TaskDefinition>();
        }
    }

}
