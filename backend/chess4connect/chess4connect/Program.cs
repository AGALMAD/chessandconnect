using chess4connect;
using chess4connect.Mappers;
using chess4connect.MiddleWares;
using chess4connect.Models.Database;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

public class Program {

    public static async Task Main(string[] args)
    {
        // Configuramos cultura invariante para que al pasar los decimales a texto no tengan comas
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

        // Configuramos para que el directorio de trabajo sea donde está el ejecutable
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);


        var builder = WebApplication.CreateBuilder(args);


        // *** Añadimos servicios al contenedor del inyector de dependencias ***

        // Añadimos la configuración guardada en el appsetting.json
        builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SECTION_NAME));


        // Añadimos controladores.
        builder.Services.AddControllers();

        // Contesto de la base de datos y repositorios
        builder.Services.AddScoped<ChessAndConnectContext>();
        builder.Services.AddScoped<UnitOfWork>();

        // Servicios
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<PasswordService>();
        builder.Services.AddScoped<FriendshipService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<ImageService>();




        // Mappers
        builder.Services.AddTransient<UserMapper>();
        builder.Services.AddTransient<PlayMapper>();
        builder.Services.AddTransient<FriendMapper>();

        //Administrador de todos los websockets
        builder.Services.AddSingleton<ConnectionManager>();
        //MiddleWare
        builder.Services.AddTransient<WebSocketMiddleWare>();


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // Configuración para poder usar JWT en las peticiones de Swagger
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                Description = "Escribe **_SOLO_** tu token JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>(true, JwtBearerDefaults.AuthenticationScheme);
        });

        builder.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                // Por seguridad guardamos la clave privada en el appsettings.json
                // La clave debe tener más de 256 bits
                Settings settings = builder.Configuration.GetSection(Settings.SECTION_NAME).Get<Settings>();
                string key = settings.JwtKey;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    // Si no nos importa que se valide el emisor del token, lo desactivamos
                    ValidateIssuer = false,
                    // Si no nos importa que se valide para quién o
                    // para qué propósito está destinado el token, lo desactivamos
                    ValidateAudience = false,
                    // Indicamos la clave
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });


        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }






        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //MiddleWare 
        app.UseMiddleware<WebSocketMiddleWare>();

        // Habilita el uso de websockets
        app.UseWebSockets();

        app.UseHttpsRedirection();

        // Indicamos que active el servicio para archivos estáticos (wwwroot)
        app.UseStaticFiles();


        // Configuramos Cors para que acepte cualquier petición de cualquier origen (no es seguro)
        app.UseCors(options =>
            options.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

        // Habilita la autenticación
        app.UseAuthentication();
        // Habilita la autorización
        app.UseAuthorization();

        // Inicializamos la base de datos
        await InitDatabaseAsync(app.Services);


        // Empezamos a atender a las peticiones de nuestro servidor
        await app.RunAsync();
    }
    static async Task InitDatabaseAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        using ChessAndConnectContext dbContext = scope.ServiceProvider.GetService<ChessAndConnectContext>();
        PasswordService passwordService = scope.ServiceProvider.GetService<PasswordService>();



        // Si no existe la base de datos entonces la creamos y ejecutamos el seeder
        if (dbContext.Database.EnsureCreated())
        {
            Seeder seeder = new Seeder(dbContext,passwordService);
            await seeder.SeedAsync();
        }
    }

}