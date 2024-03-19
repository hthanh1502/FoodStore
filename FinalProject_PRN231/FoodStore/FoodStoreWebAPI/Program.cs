using BusinessObjects.Models;
using DataAccess.Repository.Accounts;
using DataAccess.Repository.Categories;
using DataAccess.Repository.OrderDetails;
using DataAccess.Repository.Orders;
using DataAccess.Repository.Products;
using DataAccess.Repository.Roles;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FoodStoreContext>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IOrderRepositoty, OrderRepository>();
builder.Services.AddSingleton<IOrderDetailsRepository, OrderDetailsRepository>();
builder.Services.AddSingleton<IRoleRepository, RoleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();

app.Run();
