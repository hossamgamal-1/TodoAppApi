using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TodoAppApi.Helpers;

public class JwtConfigurator
{
    public static void Configure(WebApplicationBuilder builder)
    {
        var authBuilder = AddAuthentication(builder);
        AddJWTBearer(ref authBuilder,builder);
    }

    private static AuthenticationBuilder AddAuthentication(WebApplicationBuilder builder)
    {
        var authBuilder = builder.Services.AddAuthentication(
         options => {
             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }
         );
        return authBuilder;
    }

    private static void AddJWTBearer(ref AuthenticationBuilder authBuilder,WebApplicationBuilder builder)
    {
        authBuilder.AddJwtBearer(
            options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = GetTokenValidationParameters(ref builder);
            }
         );
    }

    private static TokenValidationParameters GetTokenValidationParameters(ref WebApplicationBuilder builder)
    {
        byte[] key = Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]!);

        var tokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero,
        };

        return tokenValidationParameters;
    }
}