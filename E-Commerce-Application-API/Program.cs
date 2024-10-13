using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using E_Commerce_Application_API.Data;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Mappers;
using E_Commerce_Application_API.Repositories;
using E_Commerce_Application_API.Security;
using E_Commerce_Application_API.SeedHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<Seed>();
// Register repositories and other services
// * Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomMapper, CustomMapper>();

// Register AWS Services :
// Retrieve AWS options from configuration
var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Region = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]);

// Set the credentials manually
awsOptions.Credentials = new BasicAWSCredentials(
    builder.Configuration["AWS:AccessKey"],
    builder.Configuration["AWS:SecretKey"]);

// Register AWS services with the configured options
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();

// Register JWT service
builder.Services.AddSingleton<JwtService>();

// Add Cache availability of the API
builder.Services.AddMemoryCache();

// Adding JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add authorization services
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Useful for debugging
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // Serve static files from wwwroot by default

// Serve Static Files from Angular Dist Folder**
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "E-Commerce-Application-UI", "dist", "e-commerce-app", "browser")),
    RequestPath = ""
});

// **Add Routing**
app.UseRouting();

// Add cores to the project
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// **Map API Controllers**
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    // **Catch-All Route for Angular**
    endpoints.MapFallbackToFile("index.html", new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "E-Commerce-Application-UI", "dist", "e-commerce-app", "browser")),
    });
});

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    await SeedData(app);

app.Run();

// **3. Seed Data Method**

async Task SeedData(IHost app)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<Seed>();
    await service.SeedDataContext();
}