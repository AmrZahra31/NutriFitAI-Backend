
using FitnessApp.Bl;
using FitnessApp.DAL.context;
using FitnessApp.DAL.Repositories;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


            builder.Services.AddDbContext<FitnessAppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<ApplicationUsers, IdentityRole>(options =>
            {
                /*options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;*/
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<FitnessAppContext>().AddDefaultTokenProviders(); ;



            builder.Services.AddSingleton<EmailSender>();
            builder.Services.AddHttpContextAccessor();



            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserSettingeRepository,UserSettingRepository>();
            builder.Services.AddScoped<IDietPlanRepository, DietPlanRepository>();
            builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            builder.Services.AddScoped<EmailSettings>();
            builder.Services.AddScoped<IGenDietRepository,GenDietRepository>();
            builder.Services.AddScoped<IDietService, DietService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISettingService, SettingService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IPersonalInfoService, PersonalInfoService>();
            builder.Services.AddScoped<ISignUpService, SignUpService>();
            builder.Services.AddScoped<IWorkoutService, WorkoutService>();
            builder.Services.AddScoped<IAdminService,AdminService>();

            builder.Services.AddHttpClient<IDietApiClient, DietApiClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["AiExternalApi:BaseUrl"]);
                client.Timeout = Timeout.InfiniteTimeSpan; 
            });

            builder.Services.AddHttpClient<IMacrosApiClient,MacrosApiClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["MacrosApi:BaseUrl"]);
                client.Timeout = TimeSpan.FromSeconds(30);
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Limitless Fitness App Api",
                    
                    Contact = new OpenApiContact
                    {
                        Email = "amrzahra031@gmail.com",
                    },
                    
                });
            });



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),

                    ValidateLifetime = false,
                };
            });

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{


                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/Swagger.json", "v1");
                    
                });


            //}

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
