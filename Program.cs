using EmailValidator.Model;
using EmailValidator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((options) =>
{
    options.AddServerHeader = false;
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(400);
});

builder.Services.AddTransient<IEmailValidatorService, EmailValidatorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
