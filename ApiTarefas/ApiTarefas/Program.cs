using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiTarefasContext>(opt => opt.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/tarefas", async (ApiTarefasContext db) =>
{
    return await db.Tarefas.ToListAsync();
});

app.MapPost("/tarefas", async (Tarefa tarefa, ApiTarefasContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"tarefas/{tarefa.Id}", tarefa);

});

app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool IsConcluido { get; set; }
}

class ApiTarefasContext :DbContext
{
    public ApiTarefasContext(DbContextOptions<ApiTarefasContext> options) : base(options)
    {
    }
    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}