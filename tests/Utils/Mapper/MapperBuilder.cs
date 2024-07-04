using Application.Services.Mapper;
using AutoMapper;
using Utils.HashIds;

namespace Utils.Mapper;

public class MapperBuilder
{
    public static IMapper Instance()
    {
        var hashids = HashIdsBuilder.Instance().Build();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperConfiguration(hashids));
        });

        return config.CreateMapper();
    }
}
