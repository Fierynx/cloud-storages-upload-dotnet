using NextCloudUpload;

var builder = WebApplication.CreateBuilder(args);

// Options
builder.Services.Configure<NextCloudOptions>(
    builder.Configuration.GetSection("NextCloud")
);

// DI registration
builder.Services.AddSingleton<INextCloudService, NextCloudService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowSwaggerUI");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
