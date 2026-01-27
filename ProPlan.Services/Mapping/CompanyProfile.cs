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
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDtoForRead>();
            CreateMap<CompanyDtoForCreate, Company>();
            CreateMap<CompanyDtoForUpdate, Company>();
        }
    }

}
