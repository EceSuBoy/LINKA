using Linka.Message.DAL.Context;
using Linka.Message.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];
        options.Audience = "ResourceMessage";
        options.RequireHttpsMetadata = false;
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<MessageContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IUserMessageService, UserMessageService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
