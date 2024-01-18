using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieMerchShop.Data;
using MovieMerchShop.Enum;
using MovieMerchShop.Model;
using MovieMerchShop.Service;
using MovieMerchShop.Service.Authentication;
using MovieMerchShop.Service.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AddServices();
ConfigureSwagger();
AddDbContext();
AddAuthentication();
AddIdentity();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

//AddRoles();
//AddAdmin();
//CreateMerchandise();

app.MapControllers();

app.Run();



void AddServices()
{
    builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
    builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    builder.Services.AddScoped<IOmdbApiProvider, OmdbApi>();
    builder.Services.AddScoped<IJsonProcessorOmdbApi, JsonProcessorOmdbApi>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<MovieService>();
    builder.Services.AddScoped<IMerchItemRepository, MerchItemRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
}


void ConfigureSwagger()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}


void AddDbContext()
{

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped
    );
    builder.Services.AddDbContext<UsersContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}


void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
                ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
                 IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtIssuerSigningKey"])),
            };
        });
}


void AddIdentity()
{
    builder.Services
        .AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false; 
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false; 
            options.Password.RequireUppercase = false; 
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<UsersContext>();
}



void AddRoles()
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var tAdmin = CreateAdminRole(roleManager);
    tAdmin.Wait();

    var tUser = CreateUserRole(roleManager);
    tUser.Wait();
}


async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole(builder.Configuration["RoleSettings:AdminRole"]!));
}

async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole(builder.Configuration["RoleSettings:UserRole"]!));
}

void AddAdmin()
{
    var tAdmin = CreateAdminIfNotExists();
    tAdmin.Wait();
}

async Task CreateAdminIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
    if (adminInDb == null)
    {
        var admin = new ApplicationUser { UserName = "admin", Email = "admin@admin.com", BirthDate = new DateTime(2000,12,12), Address = "admin"};
        var adminCreated = await userManager.CreateAsync(admin, "admin123");

        if (adminCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}


void CreateMerchandise()
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var movieService = serviceProvider.GetRequiredService<MovieService>();
        List<Guid> movieIds = movieService.GetMovieIds();
        List<Color> colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
        List<TShirtSize> tshirtSizes = Enum.GetValues(typeof(TShirtSize)).Cast<TShirtSize>().ToList();
        List<Material> materials = Enum.GetValues(typeof(Material)).Cast<Material>().ToList();
        List<PosterSize> posterSizes = Enum.GetValues(typeof(PosterSize)).Cast<PosterSize>().ToList();
        foreach (var movie in movieIds) 
        {
     
            //tshirt
            foreach (var color in colors)
            { 
                Random random = new Random();

                foreach (var size in tshirtSizes)
                {
                    context.MerchItems.Add(new Shirt
                    {
                        Price = 15m,
                        MovieId = movie,
                        Quantity = random.Next(5, 11),
                        Size = size,
                        Color = color
                    });
                    context.SaveChanges();
                }
            }
            //Mug
            foreach (var color in colors)
            {
                Random random = new Random();
                
                context.MerchItems.Add(new Mug
                {
                    Price = 10m,
                    MovieId = movie,
                    Quantity = random.Next(5, 11),
                    Color = color
                });
                context.SaveChanges();
            }
        
            //Poster
            foreach (var material in materials)
            {
                Random random = new Random();

                foreach (var size in posterSizes)
                {
                    context.MerchItems.Add(new Poster()
                    {
                        Price = 8m,
                        MovieId = movie,
                        Quantity = random.Next(5, 11),
                        Size = size,
                        Material = material,
                    });
                    context.SaveChanges();
                }
            }
        }

        context.SaveChanges();
    }
}

public partial class Program
{
}