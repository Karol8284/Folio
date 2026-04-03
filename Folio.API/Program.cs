using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Interfaces.Services;
using Folio.Infrastructure.Data;
using Folio.Infrastructure.Repositories;
using Folio.Infrastructure.Security;
using Folio.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// 1. DATABASE CONFIGURATION
// ============================================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ============================================================================
// 2. JWT AUTHENTICATION CONFIGURATION
// ============================================================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ============================================================================
// 3. REPOSITORY DEPENDENCY INJECTION
// ============================================================================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IReadingProgressRepository, ReadingProgressRepository>();

// ============================================================================
// 4. SERVICE DEPENDENCY INJECTION
// ============================================================================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IReadingProgressService, ReadingProgressService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ============================================================================
// 5. SECURITY & UTILITIES
// ============================================================================
builder.Services.AddScoped<JwtTokenProvider>();

// ============================================================================
// 6. API & CORS CONFIGURATION
// ============================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins("https://localhost:7001", "http://localhost:5001") // Blazor WASM dev ports
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ============================================================================
// BUILD & CONFIGURE PIPELINE
// ============================================================================
var app = builder.Build();

// Apply migrations automatically on startup (development only)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }

    app.MapOpenApi();
}

// ============================================================================
// MIDDLEWARE PIPELINE
// ============================================================================
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowBlazor");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ============================================================================
// RUN APPLICATION
// ============================================================================
app.Run();

