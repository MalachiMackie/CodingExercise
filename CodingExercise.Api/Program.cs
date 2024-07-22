using CodingExercise.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPayslipGeneratorService, PayslipGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// enable for production:
// app.UseHttpsRedirection();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();

// partial class for program so that IntegrationTests have a class to point against
public partial class Program
{
}