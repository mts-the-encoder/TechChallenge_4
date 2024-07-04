using Application.Services.Token;

namespace Utils.Token;

public class TokenServiceBuilder
{
    public static TokenService Instance()
    {
        return new TokenService(1000,"PCNdci1ZKX4wdTU1I2tzSThoPi9pTX0idVpKfHlXeHJiZ3JZ");
    }
}