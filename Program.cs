using Challenge_Sprints1e2.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

//Iremos realizar a injeção da dependencia de DBCONTEXT
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString, b => b.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancelationToken) =>
    {
        document.Info.Title = "ClyvoVet API";

        document.Info.Version = "v1";

        document.Info.Description =
        """
        API RESTful do sistema ClyvoVet.

        Responsável pelo gerenciamento de:
        • Usuários
        • Tutores
        • Veterinários
        """;

        return Task.CompletedTask;
    });
});
//builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//-------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();