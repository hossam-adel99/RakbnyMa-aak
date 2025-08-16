using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using RakbnyMa_aak.Behaviors;
using RakbnyMa_aak.CQRS.Features.Auth.Commands.RegisterDriver;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Filters;
using RakbnyMa_aak.Hubs;
using RakbnyMa_aak.Mapping;
using RakbnyMa_aak.MiddleWares;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Implementations;
using RakbnyMa_aak.Repositories.Interfaces;
using RakbnyMa_aak.SeedData;
using RakbnyMa_aak.Services.Implementations;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.SignalR;
using RakbnyMa_aak.UOW;
using System.Text;

namespace RakbnyMa_aak
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Environment.SetEnvironmentVariable("EPPlusLicenseContext", "NonCommercial");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
            var builder = WebApplication.CreateBuilder(args);

           

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Configuration.GetSection("Cloudinary");

            // Register payment service HERE (BEFORE builder.Build())
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddScoped<IPaymentService, MockPaymentService>();
            }
            else
            {
                builder.Services.AddHttpClient<IPaymentService, PaymentGatewayService>(client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["PaymentGateway:BaseUrl"]);
                    client.Timeout = TimeSpan.FromSeconds(30);
                });
            }

            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<IDriverVerificationService, DriverVerificationService>();
            builder.Services.AddHttpClient<IDriverVerificationService, DriverVerificationService>();
            builder.Services.AddMediatR(typeof(RegisterDriverCommand).Assembly);
            builder.Services.AddScoped<IDriverRegistrationService, DriverRegistrationService>();
            builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddScoped<IDriverRepository, DriverRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ITripRepository, TripRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IGovernorateRepository, GovernorateRepository>();
            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<ITripTrackingRepository, TripTrackingRepository>();


            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IExcelExportService, ExcelExportService>();
            builder.Services.AddScoped<AdminReportService>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // 2. Configure Identity with ApplicationUser
            builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;

                options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ " +
        "ابتثجحخدذرزسشصضطظعغفقكلمنهويءئةؤإأآى ";
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddHttpContextAccessor();

            // 3. Add controllers and Swagger
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelAttribute>();
            })
             .AddJsonOptions(opt =>
             {
                 opt.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
             });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            /****** Swagger & OpenAPI  ******/
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = " ITI Project"
                });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                         new string[] {}
                     }
                 });
            });

            // 4. Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();
            builder.Services.AddScoped<TransactionMiddleware>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = builder.Configuration["Jwt:Issuer"],
                         ValidateAudience = true,
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                         ),
                         ValidateLifetime = true
                     };
                 });

            builder.Services.AddSignalR();
            builder.Services.AddAuthorization();

            builder.Services.AddHangfire(config =>
               config.UseSqlServerStorage(builder.Configuration.GetConnectionString("SQLConnectionString")));

            builder.Services.AddHangfireServer();


            var app = builder.Build();

            //Seed the database with initial data
            // 1. Add DbContext
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await DbSeeder.SeedRolesAsync(roleManager);

                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await DbSeeder.SeedGovernoratesAndCitiesAsync(context);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                await DbSeeder.SeedAdminUserAsync(userManager, roleManager, config);
            }

            // 5. Use Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rakbny Maak API V1");
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();

            // 6. Use CORS
            app.UseCors("AllowAll");

            //Add custom middlewares
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            app.UseMiddleware<TransactionMiddleware>();

            // 7. Use Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter() }
            });

            app.MapHub<NotificationHub>("/notificationHub");
            app.MapHub<TripTrackingHub>("/tripTrackingHub");
            app.MapHub<ChatHub>("/chatHub");

            app.MapGet("/", () => Results.Ok("API is running..."));

            // 8. Map Controllers
            app.MapControllers();

            app.Run();
        }
    }
}