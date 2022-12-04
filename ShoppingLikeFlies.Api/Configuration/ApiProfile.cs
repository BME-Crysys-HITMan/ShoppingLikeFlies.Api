using ShoppingLikeFiles.DomainServices.DTOs;

namespace ShoppingLikeFlies.Api.Configuration
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<CaffDTO, CaffResponse>()
                .ForCtorParam("id", _=>_.MapFrom(c=>c.Id))
                .ForCtorParam("caption", _ => _.MapFrom(c => c.Caption))
                .ForCtorParam("tags", _ => _.MapFrom(c => c.Tags))
                .ForCtorParam("comments", _ => _.MapFrom(c => c.Comments))
                .ForCtorParam("creator", _ => _.MapFrom(c => c.Creator))
                .ForCtorParam("previewUrl", _ => _.MapFrom(c => c.ThumbnailPath));
            CreateMap<CaffDTO, CaffAllResponse>();
            CreateMap<UpdateCaffRequest, CaffDTO>();
            CreateMap<CommentDTO, CommentResponse>()
                .ForMember(d=>d.Text, _=>_.MapFrom(s=>s.Text))
                .ForMember(d=>d.UserId, _=>_.MapFrom(s=>s.UserId));
        }
    }
}
