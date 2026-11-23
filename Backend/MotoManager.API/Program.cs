using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtKey = builder.Configuration["jwt:Key"];
var issuer = builder.Configuration["jwt:Issuer"];
var audience = builder.Configuration["jwt:Audience"];
var expiryMinuts = builder.Configuration.GetValue<int>("jwt:ExpiryMinuts");



builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Name = "Authorization",



        Type = SecuritySchemeType.Http,


        Scheme = "Bearer",


        BearerFormat = "JWT",


        In = ParameterLocation.Header,


        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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




builder.Services.AddCors(option =>
{
    option.AddPolicy("MotoManagerApiCorsPolicy", policy =>
    {
        policy
        .WithOrigins("https://localhost:7063", "http://localhost:5237"

            ).AllowAnyHeader().AllowAnyMethod();
    });
});




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
                                            {
                                                ValidateIssuer = true,


                                                ValidateAudience = true,


                                                ValidateLifetime = true,


                                                ValidateIssuerSigningKey = true,


                                                ValidIssuer = issuer,


                                                ValidAudience = audience,


                                                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
                                            };
    });




builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddControllers();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MotoManagerApiCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

