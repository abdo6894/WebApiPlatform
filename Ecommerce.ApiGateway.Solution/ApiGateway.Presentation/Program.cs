using ApiGateway.Presentation.Middlware;
using ecommrece.sharedliberary.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add Ocelot with caching
builder.Services.AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle());

// Add JWT authentication
JwtAuthentiactionSchema.AddJwtAuthentiactionSchema(builder.Services, builder.Configuration);

// Enable CORS
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add built-in authentication/authorization services
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Enable CORS
app.UseCors();

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Use Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Custom Middleware
app.UseMiddleware<AttachSignatureToRequest>();

// Use Ocelot Gateway (must be last)
await app.UseOcelot();

app.Run();
