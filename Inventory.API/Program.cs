using Inventory.API.Configurations;
using Inventory.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddServices();

builder.Services.AddAuthorization(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

app.Services.MigrateSecurityDbContext();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Middlewares
app.UseMiddleware<OrganizationTenantMiddleware>();

app.Run();