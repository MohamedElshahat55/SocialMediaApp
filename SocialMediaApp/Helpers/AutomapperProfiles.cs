using AutoMapper;
using SocialMediaApp.DTOs;
using SocialMediaApp.Entities;
using SocialMediaApp.Extensions;

namespace SocialMediaApp.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(des=>des.PhotoUrl,opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
                .ForMember(des=>des.Age,opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
            
            CreateMap<Photo, PhotoDto>();
        }
    }
}
