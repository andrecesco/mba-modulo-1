﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MLV.ApiRest.Extensions;
using MLV.Business.Data;
using System.Text;

namespace MLV.ApiRest.Configuration;

public static class IdentityConfig
{
    public static WebApplicationBuilder AddIdentityConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<MlvDbContext>()
            .AddErrorDescriber<IdentityMensagensPortugues>();

        // Pegando o Token e gerando a chave encodada
        var JwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtSettings>(JwtSettingsSection);

        var jwtSettings = JwtSettingsSection.Get<JwtSettings>();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Segredo);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audiencia,
                ValidIssuer = jwtSettings.Emissor
            };
        });

        builder.Services.AddHttpContextAccessor();

        return builder;
    }
}
