using System.Text;
using Filio.Api.Data;
using Filio.Api.Services.Auth;
using Filio.Api.Settings.ApiKey;
using Filio.Api.Settings.Jwt;
using Filio.Api.Settings.Swagger;
using Filio.Common.Extensions;
using Filio.FileLib.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.UseAllOfForInheritance();
});

builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));
var jwt = jwtSection.Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(jwtSection);
builder.Services.AddSingleton(s => s.GetRequiredService<IOptions<JwtSettings>>().Value);

var apiKeySection = builder.Configuration.GetSection(nameof(ApiKeySettings));
builder.Services.Configure<ApiKeySettings>(apiKeySection);
builder.Services.AddSingleton(s => s.GetRequiredService<IOptions<ApiKeySettings>>().Value);


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql
        (builder.Configuration.GetConnectionString("PostgreSQL"),
         npgsqlOptions =>
         {
             npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3,
                                                                maxRetryDelay: TimeSpan.FromSeconds(1),
                                                                errorCodesToAdd: null);
         });
});

builder.Services.AddCommon()
                .AddFileDetector()
                .AddImageLib()
                .Register();

builder.Services.AddSingleton<IAuthService, AuthService>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = builder.Environment.IsProduction();
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningSecret)),
                        TokenDecryptionKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.EncryptionSecret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

builder.Services.AddAuthorization();

builder.Services.AddFileLib(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
