using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Token;

public class TokenService
{
    private static string EmailAlias = "eml";
    private readonly double _tokenLifeTimeMinutes;
    private readonly string _securityKey;
    public TokenService(double tokenLifeTimeMinutes, string securityKey)
    {
        _tokenLifeTimeMinutes = tokenLifeTimeMinutes;
        _securityKey = securityKey;
    }

    public string GetEmail(string token)
    {
        var claims = ValidateToken(token);

        return claims.FindFirst(EmailAlias).Value;
    }

    public string GenerateToken(string userEmail)
    {
        var claims = new List<Claim>()
        {
            new Claim(EmailAlias, userEmail)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tokenLifeTimeMinutes),
            SigningCredentials = new SigningCredentials(SymmetricKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters()
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SymmetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false
        };

       var claims = tokenHandler.ValidateToken(token, validationParameters, out _);

       return claims;
    }

    private SymmetricSecurityKey SymmetricKey()
    {
        var symmetricKey = Convert.FromBase64String(_securityKey);
        return new SymmetricSecurityKey(symmetricKey);
    }
}