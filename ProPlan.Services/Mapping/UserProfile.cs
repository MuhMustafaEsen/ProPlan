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
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDtoForRead>();

            CreateMap<UserDtoForCreate, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
           
            CreateMap<UserDtoForUpdate, User>();
        }
    }


    //ileride Password ve passwordhash için yeni kod bu olacak : 
    /*
     * Hash işlemini Service’te yapsak daha iyi olur.
        Entity’de plaintext password tutulmamalıyız kanımca.
     public UserProfile()
    {
        CreateMap<User, UserDtoForRead>();

        CreateMap<UserDtoForCreate, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UserDtoForUpdate, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
     */
}
