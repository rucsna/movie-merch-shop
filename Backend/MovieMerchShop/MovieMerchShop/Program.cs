using Microsoft.EntityFrameworkCore;
using MovieMerchShop.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOmdbApiProvider, OmdbApi>();
builder.Services.AddScoped<IJsonProcessorOmdbApi, JsonProcessorOmdbApi>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});


app.MapControllers();

app.Run();