using GameServer.Data;
using GameServer.Data.Repositories;
using GameServer.Data.Repositories.DomainRepositories;
using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Data.UnitOfWork;
using GameServer.Models;
using GameServer.Services;
using GameServer.Services.AppSettings;
using GameServer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SquadronGameServer.SignalR;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GameServerDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<GameServerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<HttpContextAccessor>();

//in memory caching used to know how many players are playing at the moment
builder.Services.AddMemoryCache();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IAnswerRepository, AnswerRepository>();
builder.Services.AddTransient<IGameRepository, GameRepository>();
builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();
builder.Services.AddTransient<IRoundRepository, RoundRepository>();

//unit of work
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

//services
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddTransient<IRoundService, RoundService>();
builder.Services.AddTransient<IAnswerService, AnswerService>();

//options pattern injections
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});
var frontendApp = "_frontendApp";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: frontendApp,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200");
                          builder.AllowAnyMethod();
                          builder.AllowAnyHeader();
                          builder.AllowCredentials();
                      });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(frontendApp);
app.UseHttpsRedirection();

app.UseAuthentication();


app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<RoundHub>("api/signalR");
    });

app.Run();
