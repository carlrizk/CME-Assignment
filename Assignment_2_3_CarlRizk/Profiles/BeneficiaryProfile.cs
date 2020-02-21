using AutoMapper;

namespace Assignment_2_3_CarlRizk.Profiles
{
    public class BeneficiaryProfile : Profile
    {
        public BeneficiaryProfile()
        {
            CreateMap<Models.BeneficiaryInputDto, Entities.Beneficiary>();
        }
    }
}
