using FileCloudUpload;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FileCloudOptions>(builder.Configuration.GetSection("FileCloud"));

builder.Services.AddSingleton<IFileCloudService, FileCloudService>();

builder.Services.AddControllers();

// Add services to the container.
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

app.MapControllers();

app.Run();
