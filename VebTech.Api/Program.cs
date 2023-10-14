using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Text;
using VebTech.Application.Services;
using VebTech.Application.Services.Interfaces;
using VebTech.Domain.Models;
using VebTech.Domain.Models.Configurations;
using VebTech.Domain.Models.DTO;
using VebTech.Filters;
using VebTech.Infrastructure.Context;
using VebTech.Infrastructure.Repositories;
using VebTech.Infrastructure.Repositories.Abstract;
using VebTech.Validators;
using VebTech.Validators.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },

            Array.Empty<string>()
        }
    });
    option.EnableAnnotations();
});

var jwtConfig = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtConfig>()!;
builder.Services.AddSingleton(jwtConfig);

var adminConfig = builder.Configuration
    .GetSection("AdminUser")
    .Get<AdminConfig>()!;
builder.Services.AddSingleton(adminConfig);

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<UserDto, User>()
        .ForMember(user => user.Age, opt =>
            opt.MapFrom((dto, user) =>
            !string.IsNullOrEmpty(dto.Age) && int.TryParse(dto.Age, out int age) ? age : user.Age))
        .ForAllMembers(options => options.Condition((dto, user, srcMember) => srcMember != null));

    cfg.CreateMap<AdminDto, Admin>()
        .ForMember(admin =>
            admin.Password, opt =>
            opt.MapFrom(dto => BCrypt.Net.BCrypt.HashPassword(dto.Password)));

    cfg.CreateMap<RoleDto, Role>()
    .ForMember(role => role.UserId, opt =>
        opt.MapFrom((dto, role) =>
        !string.IsNullOrEmpty(dto.UserId) && int.TryParse(dto.UserId, out int userId) ? userId : role.UserId));
});
var mapper = new Mapper(mapperConfig);
builder.Services.AddSingleton(mapper);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
        };
    });

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
    .AddJsonFile("seri-log.config.json")
    .Build())
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IValidate, Validate>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();