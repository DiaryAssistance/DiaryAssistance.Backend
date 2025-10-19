using DiaryAssistance.Api.Middlewares;
using DiaryAssistance.Application;
using DiaryAssistance.Application.Options;
using DiaryAssistance.Infrastructure;
using DiaryAssistance.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration
        .GetRequiredSection(JwtSettings.SectionName)
        .Get<JwtSettings>()!;

    options.TokenValidationParameters.ValidIssuer = jwtSettings.Issuer;
    options.TokenValidationParameters.ValidAudience = jwtSettings.Audience;
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

await app.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();