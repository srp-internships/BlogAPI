using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;
using AutoMapper;

namespace Ali_Mav.BlogAPI.Data.Mappar
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<UserViewModel, User>()
                 .ForMember(dst => dst.Address,
              opt => opt.MapFrom(src => src.Address.City + " " + src.Address.Street))
                 .ForMember(dst=>dst.FirstName, opt=>opt.MapFrom(src=>src.Name))
                 .ForMember(dst=>dst.FullName,opt=>opt.MapFrom(src=>src.Username +" " +src.Name))
                 .ForMember(dst => dst.CompanyName,opt => opt.MapFrom(src => src.Company.Name))
                 .ForMember(dst => dst.LastName, opt => opt.MapFrom(src=>src.Username));

            this.CreateMap<Post, PostGetDto>()
                .ForMember(dst => dst.UserFullName,
                opt => opt.MapFrom(src => src.User.FullName));

            this.CreateMap<Post, PostCreateDto>();
            this.CreateMap<PostCreateDto, Post>();
            this.CreateMap<Post, PostUpdateDto>();
            this.CreateMap<PostUpdateDto, Post>();


        }
    }
}
