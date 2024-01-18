using System.Net.Http.Headers;
using System.Text;
using fasad;
using fasad.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddSingleton<FileService>();

builder.Services.AddHttpClient<NextcloudService>(o =>
{
    o.BaseAddress = new Uri("http://localhost:8080/remote.php/dav/");
    o.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("admin:admin")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/arende/{arendenummer}", async ([FromServices] NextcloudService service, [FromRoute] string arendenummer) =>
{
    var result = await service.CreateArende(arendenummer);

    return result ? Results.Created() : Results.BadRequest();
});

app.MapPost("/arende/{arendenummer}/upload", async ([FromServices] NextcloudService service, [FromBody] UploadRequest request) =>
{
    var result = await service.UploadDocument(request);

    return result ? Results.Created() : Results.BadRequest();
});

app.MapPost("/arende/{arendenummer}/convert", async ([FromServices] NextcloudService service, [FromBody] ConvertRequest request) =>
{
    var result = await service.ConvertToAllmanHandling(request);
    
    return result ? Results.Created() : Results.BadRequest();
});

app.Run();