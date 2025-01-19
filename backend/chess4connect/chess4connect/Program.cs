using chess4connect;
using chess4connect.Mappers;
using chess4connect.Models;
using chess4connect.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


Directory.SetCurrentDirectory(AppContext.BaseDirectory);

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<Settings>>().Value);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// CONFIGURANDO JWT
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,

            // INDICAMOS LA CLAVE
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ChessAndConnectContext>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<FriendshipService>();
builder.Services.AddScoped<UserMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Permite CORS
app.UseCors();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



using (IServiceScope scope = app.Services.CreateScope())
{
    ChessAndConnectContext dbContext = scope.ServiceProvider.GetService<ChessAndConnectContext>();
    if (dbContext.Database.EnsureCreated())
    {
        PasswordService passwordService = new PasswordService();

        User admin = new User
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            Password = passwordService.Hash("admin"),
            Role = "admin",
            AvatarImageUrl = "",
            Banned = false,
        };

        dbContext.Users.Add(admin);
        dbContext.SaveChanges();


    }





}

app.Run();
