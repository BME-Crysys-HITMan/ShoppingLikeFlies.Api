using ShoppingLikeFiles.DomainServices.DTOs;

namespace ShoppingLikeFlies.Api.Configuration;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<CaffDTO, CaffResponse>()
            .ForMember(d => d.Id, _ => _.MapFrom(s => s.Id))
            .ForMember(d => d.Caption, _ => _.MapFrom(s => s.Caption))
            .ForMember(d => d.Tags, _ => _.MapFrom(s => s.Tags))
            .ForMember(d => d.Comments, _ => _.MapFrom(s => s.Comments))
            .ForMember(d => d.Creator, _ => _.MapFrom(s => s.Creator))
            .ForMember(d => d.PreviewUrl, _ => _.MapFrom(s => s.ThumbnailPath));
        CreateMap<CaffDTO, CaffAllResponse>()
            .ForMember(d => d.Id, _ => _.MapFrom(s => s.Id))
            .ForMember(d => d.Caption, _ => _.MapFrom(s => s.Caption))
            .ForMember(d => d.Tags, _ => _.MapFrom(s => s.Tags))
            .ForMember(d => d.Comments, _ => _.MapFrom(s => s.Comments))
            .ForMember(d => d.Creator, _ => _.MapFrom(s => s.Creator))
            .ForMember(d => d.PreviewUrl, _ => _.MapFrom(s => s.ThumbnailPath));
        CreateMap<UpdateCaffRequest, CaffDTO>();
        CreateMap<CommentDTO, CommentResponse>()
            .ForMember(d => d.Text, _ => _.MapFrom(s => s.Text))
            .ForMember(d => d.UserId, _ => _.MapFrom(s => s.UserId));
    }
}
