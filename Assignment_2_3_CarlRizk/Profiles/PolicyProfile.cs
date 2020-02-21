using Assignment_2_3_CarlRizk.Entities;
using AutoMapper;

namespace Assignment_2_3_CarlRizk.Profiles
{
    public class PolicyProfile : Profile
    {
        public PolicyProfile()
        {
            CreateMap<Models.PolicyInputForCreationDto, Policy>();
            CreateMap<Entities.Policy, Models.PolicyOutputDto>();
        }

    }
}
