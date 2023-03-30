
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using portfolio_api.Configuration;
using portfolio_api.Contracts;
using portfolio_api.Data;
using portfolio_api.Middleware;
using portfolio_api.Repository;
using Serilog;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowLocalhost", b => b.AllowCredentials().AllowAnyHeader()
        .AllowAnyMethod().SetIsOriginAllowed(origin => new Uri(origin).Host == "app.localhost"));
});

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowMarekpuu", b => b.AllowCredentials().AllowAnyHeader()
        .AllowAnyMethod().SetIsOriginAllowed(origin => new Uri(origin).Host == "app.marekpuu"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.Authority = builder.Configuration.GetConnectionString("Auth0:Authority");
        options.Audience = builder.Configuration.GetConnectionString("Auth0:Audience");
    }
    else
    {
        options.Authority = builder.Configuration.GetValue<string>("Auth0Authority");
        options.Audience = builder.Configuration.GetValue<string>("Auth0Audience");
    }
});

builder.Services.AddDbContext<MarekPuuDbContext>(options =>
{
    if (builder.Environment.IsDevelopment()) { options.UseNpgsql(builder.Configuration.GetConnectionString("MarekPuuDbConnectionStringLocal")); }
    else { options.UseNpgsql(builder.Configuration.GetValue<string>("MarekPuuDbConnectionString")); }
}
);


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IHouseholdRepository, HouseholdRepository>();
builder.Services.AddScoped<IAuthServerUserRepository, AuthServerUserRepository>();
builder.Services.AddScoped<IHouseholdUserRepository, HouseholdUserRepository>();

builder.Services.AddAutoMapper(typeof(MapperConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalhost");
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowMarekpuu");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
