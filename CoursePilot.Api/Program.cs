var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//logs
app.Use(async (context, next) =>
{
    Console.WriteLine($"{DateTime.UtcNow:u} {context.Request.Method} {context.Request.Path}");

    await next();

    Console.WriteLine($"{DateTime.UtcNow:u} Response: {context.Response.StatusCode}");
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
