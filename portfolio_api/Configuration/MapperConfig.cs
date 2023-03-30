using AutoMapper;
using portfolio_api.Data;
using portfolio_api.Models.HouseholdUsers;

namespace portfolio_api.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<HouseholdUser, GetUserHouseholdsDto>().ReverseMap();
            CreateMap<GetUserHouseholdsDto, HouseholdUser>().ReverseMap();
        }
    }
}
