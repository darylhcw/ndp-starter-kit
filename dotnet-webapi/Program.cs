using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DotnetWebAPI.Repository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


/*********************************************
 * Services
 *********************************************/

builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            if (builder.Environment.IsDevelopment()) {
                policy.WithOrigins("https://localhost:3000",
                                   "http://localhost:3000")
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
            } else {
                policy.WithOrigins("https://localhost:3000",
                                   "http://localhost:3000",
                                   "http://my-website.com")
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
            }
        });
});

// Database Contexts + Repositories.
bool useDockerCString = Environment.GetEnvironmentVariable("IN_DOCKER") == "true";

string notesCString = useDockerCString ? "NotesDockerConnString" : "NotesConnectionString";
string pgCString = builder.Configuration.GetConnectionString(notesCString) ?? "";
builder.Services.AddDbContext<NotesDbContext>(
    options => options.UseNpgsql(pgCString)
);
builder.Services.AddScoped<INotesRepository, NotesRepository>();


// App Docs Swagger/OpenAPi - https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{ 
    options.SwaggerDoc("v1", new OpenApiInfo
	{
        Version = "v1",
        Title = "Test API",
        Description = "An ASP.NET Core Web API",
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();


/*********************************************
 * Configure the HTTP request pipeline.
 *********************************************/

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
	app.UseSwaggerUI();
    NotesSeeder.AddNotes(app);
}


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();



