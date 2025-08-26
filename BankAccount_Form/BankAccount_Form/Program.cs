
using BankAccount_Form.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BankAccount_Form
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var connection = builder.Configuration.GetConnectionString("DB");
            builder.Services.AddDbContext<BankDbContext>(p=>p.UseSqlServer(connection));
            builder.Services.AddCors(p => p.AddPolicy("abcpolicy", q => q.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
             options.TokenValidationParameters = new TokenValidationParameters
             {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = builder.Configuration["Jwt:Issuer"],
              ValidAudience = builder.Configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
             };
          });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("abcpolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
