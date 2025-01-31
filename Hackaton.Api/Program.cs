using Hackaton.Domain.Mappings;
using Hackaton.Domain.Settings;
using Hackaton.Infra.Data.Context;
using Hackaton.Infra.Ioc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var postGreeDbSettings = builder.Configuration.GetSection("PostGreeDbSettings").Get<PostgreDbSettings>();

builder.Services.AddDbContext<FiapDbContext>(options =>
    options.UseNpgsql(postGreeDbSettings.ConnectionString));

builder.Services.AddSingleton<NpgsqlConnection>(sp => new NpgsqlConnection(postGreeDbSettings.ConnectionString));

builder.Services.AddScoped<FiapDbContext>();


builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureApplicationContext(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Hackaton Fiap API",
        Version = "v1",
        Description = "API da aplicação Hackaton Fiap"
    });
});

var app = builder.Build();

// Configure o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
