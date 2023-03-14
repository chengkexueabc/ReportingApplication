using Microsoft.EntityFrameworkCore;
using ProductWebAPI;
using ProductWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductContext>(opt =>
{
    string connectionString = builder.Configuration.GetConnectionString("MysqlConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);
});

builder.Services.AddIdentityServer()
                .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiScopes(Config.ApiScoppes)
                        .AddDeveloperSigningCredential();

builder.Services.AddAuthentication("Bearer")
           .AddJwtBearer("Bearer", options =>
{
    options.Authority = builder.Configuration["AuthorityServer"];
    options.RequireHttpsMetadata = false;

    options.Audience = "productwebapi";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
