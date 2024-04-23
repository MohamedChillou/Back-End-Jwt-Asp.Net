using Back_Auth_Jwt.Data;
using Back_Auth_Jwt.Services;
using Back_Auth_Jwt.Extention;
using Back_Auth_Jwt.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(op => {
    op.UseSqlServer(builder.Configuration.GetConnectionString("MyConn"));
});
builder.Services.AddScoped<IAuthentification, Authentification>();
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomJwt(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader(); 
        });
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.MapControllers();

app.Run();
