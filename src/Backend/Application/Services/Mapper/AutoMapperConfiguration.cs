using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using HashidsNet;

namespace Application.Services.Mapper;

public class AutoMapperConfiguration : Profile
{
    private readonly IHashids _hashIds;

    public AutoMapperConfiguration(IHashids hashIds)
    {
        _hashIds = hashIds;
        EntityToResponse();
        RequestToEntity();
    }

    private void EntityToResponse()
    {
        CreateMap<Movie, MovieResponse>()
            .ForMember(dest => dest.Id, config => config
                .MapFrom(x => _hashIds.EncodeLong(x.Id)))
            .ForMember(dest => dest.Country, cfg => cfg
                .MapFrom(x => x.Country))
			.ForMember(dest => dest.Gender, cfg => cfg
				.MapFrom(x => x.Gender)).ReverseMap();

        CreateMap<User, UserResponse>().ReverseMap()
            .ForMember(dest => dest.Password, cfg => cfg.Ignore());
    }

    private void RequestToEntity()
    {
        CreateMap<UserRequest, User>().ReverseMap();

        CreateMap<MovieRequest, Domain.Entities.Movie>()
            .ForMember(dest => dest.Gender,(IMemberConfigurationExpression<MovieRequest, Movie, object> cfg) => cfg
                .MapFrom(x => x.Gender))
			.ForMember(dest => dest.Country, (IMemberConfigurationExpression<MovieRequest, Movie, object> cfg) => cfg
				.MapFrom(x => x.Country)).ReverseMap();
    }
}