using AutoMapper;

namespace Assignment_2_3_CarlRizk.Profiles
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<Models.ClaimInputForCreationDto, Entities.Claim>();
            CreateMap<Entities.Claim, Models.ClaimOutputDto>();
            CreateMap<Models.ClaimInputForUpdateDto, Entities.Claim>();
        }

    }
}
