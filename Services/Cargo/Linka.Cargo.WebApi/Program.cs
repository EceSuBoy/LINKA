using Linka.Cargo.BusinessLayer.Abstract;
using Linka.Cargo.BusinessLayer.Concrete;
using Linka.Cargo.DataAccessLayer.Abstract;
using Linka.Cargo.DataAccessLayer.Concrete;
using Linka.Cargo.DataAccessLayer.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CargoContext>();
builder.Services.AddScoped<ICargoCompanyDal, EFCargoCompanyDal>();
builder.Services.AddScoped<ICargoCompanyService, CargoCompanyManager>();
builder.Services.AddScoped<ICargoCustomerDal, EFCargoCustomerDal>();
builder.Services.AddScoped<ICargoCustomerService, CargoCustomerManager>();
builder.Services.AddScoped<ICargoDetailDal, EFCargoDetailDal>();
builder.Services.AddScoped<ICargoDetailService, CargoDetailManager>();
builder.Services.AddScoped<ICargoOperationDal, EFCargoOperationDal>();
builder.Services.AddScoped<ICargoOperationService, CargoOperationManager>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
