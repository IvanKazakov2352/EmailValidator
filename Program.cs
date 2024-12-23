using EmailValidator.Model;
using EmailValidator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICheckEmailService, CheckEmailService>();
builder.Services.AddTransient<ICheckEmailService, CheckEmailService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
