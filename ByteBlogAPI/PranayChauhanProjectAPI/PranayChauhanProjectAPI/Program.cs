using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using PranayChauhanProjectAPI.Data;
using PranayChauhanProjectAPI.Repository.Implementation;
using PranayChauhanProjectAPI.Repository.Interface;
using PranayChauhanProjectAPI.Configuration;
using Microsoft.Win32;
using System.Net.Http;
using PranayChauhanProjectAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Add and configure Swagger middleware
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


// Register DeepSeekSettings from appsettings.json
builder.Services.Configure<DeepSeekSettings>(builder.Configuration.GetSection("DeepSeek"));
builder.Services.AddOptions<DeepSeekSettings>()
    .Bind(builder.Configuration.GetSection("DeepSeek"))
    .ValidateDataAnnotations()
    .Validate(settings => !string.IsNullOrEmpty(settings.ApiKey), "API key is required.");
    //.Validate(settings => !string.IsNullOrEmpty(settings.BaseUrl), "Base URL is required.");

// Load API key from appsettings.json
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));

// Register HttpClientFactory
builder.Services.AddHttpClient<IChatGPTRepository, ChatGPTRepository>();
builder.Services.AddHttpClient<IDeepSeekRepository, DeepSeekRepository>();


//AddScoper For Create Repository Pattern
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBlogPostsRepository, BlogPostsRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ITokenReoisitory, TokenRepository>();



//Injecting DbContext  Into Our Application 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectSSMSConnectionString"));
});



builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectSSMSConnectionString"));
    options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});



builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("PranayProject")
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 9;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            AuthenticationType = "Jwt",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});



app.MapControllers();

app.Run();
