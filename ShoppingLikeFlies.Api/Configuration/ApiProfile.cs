using ShoppingLikeFiles.DomainServices.DTOs;

namespace ShoppingLikeFlies.Api.Configuration
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<CaffDTO, CaffResponse>().ReverseMap();
            CreateMap<UpdateCaffRequest, CaffDTO>();
        }
    }
}
