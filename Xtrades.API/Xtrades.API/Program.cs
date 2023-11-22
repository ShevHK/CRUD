using Microsoft.EntityFrameworkCore;
using Xtrades.BLL.Interfaces;
using Xtrades.BLL.Services;
using Xtrades.DAL.Context;
using Xtrades.DAL.Entities;
using Xtrades.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddTransient<IRepository<User>, GeneralRepository<User>>();
builder.Services.AddTransient<IRepository<Group>, GeneralRepository<Group>>();
builder.Services.AddTransient<IRepository<UserGroup>, GeneralRepository<UserGroup>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddAutoMapper(typeof(Program)); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
