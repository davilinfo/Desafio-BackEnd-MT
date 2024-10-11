using Application.Interface;
using Application.Models.Response;
using Application.Models.ViewModel;
using Application.Services;
using Domain.Contract;
using Domain.Contract.Mongo;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Persistence.Context;
using Persistence.MongoRepository;
using Persistence.Repository;
using System.Reflection;

const string postgresConnectionString = "DefaultConnection";
const string redisConnectionString = "Redis";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(gen =>
{
    gen.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Manutenção de Motos",
        Version = "1.0.1",        
        Contact = new OpenApiContact
        {
            Name = "Davi Lima Alves",
            Url = new Uri("https://linkedin.com/in/davilalves")
        }
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    gen.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName), true);
});

builder.Services.AddDbContext<Context>(o=> o.UseNpgsql(builder.Configuration.GetConnectionString(postgresConnectionString), a=> a.EnableRetryOnFailure()));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString(redisConnectionString);
    options.InstanceName = "SampleInstance";
});
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
#region scoped repositories
builder.Services.AddScoped<IRepositoryDeliver, RepositoryDeliver>();
builder.Services.AddScoped<IRepositoryLease, RepositoryLease>();
builder.Services.AddScoped<IRepositoryMotocycleBike, RepositoryMotocycleBike>();
#endregion
#region scoped applicationServices
builder.Services.AddScoped<IApplicationServiceDeliver, ApplicationServiceDeliver>();
builder.Services.AddScoped<IApplicationServiceLease, ApplicationServiceLease>();
builder.Services.AddScoped<IApplicationServiceMotocycleBike, ApplicationServiceMotocycleBike>();
#endregion
#region scoped notifiers
builder.Services.AddScoped<INotify<MessageDeliver>, NotifyDeliver>();
builder.Services.AddScoped<INotify<MessageMoto>, NotifyMotocycleBike>();
builder.Services.AddScoped<INotify<MessageLease>, NotifyLease>();
builder.Services.AddScoped<INotify<string>, NotifyString>();
#endregion
#region singleton
builder.Services.AddSingleton<IRepositoryMongoMoto, RepositoryMongoMotocycleBike>();
builder.Services.AddSingleton<IRepositoryMongoDeliver, RepositoryMongoDeliver>();
builder.Services.AddSingleton<IRepositoryMongoLease, RepositoryMongoLease>();
#endregion
builder.Services.AddAutoMapper(Assembly.Load("Application"));
builder.Services.AddHostedService<ListenerQueueMoto>();
builder.Services.AddHostedService<ListenerQueueDeliver>();
builder.Services.AddHostedService<ListenerQueueLease>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
