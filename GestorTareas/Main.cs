class Program
{
static void Main(string[] args)
{
try
{
var tarea = new Tarea(
titulo: "Implementar login",
fechaLimite: DateTime.Today.AddDays(7),
prioridad: PrioridadTarea.Alta,
descripcion: "Formulario con validación de credenciales"
);

Console.WriteLine("Tarea creada:");
Console.WriteLine(tarea);
Console.WriteLine($"Días restantes: {tarea.DiasRestantes}");

bool iniciada = tarea.Iniciar();
Console.WriteLine($"Iniciada: {iniciada}");
Console.WriteLine($"Estado: {tarea.Estado}");

// Intentar iniciar de nuevo
bool reintento = tarea.Iniciar(); // → false
Console.WriteLine($"Reintento: {reintento}");

tarea.Completar();
Console.WriteLine($"Estado final: {tarea.Estado}");
}
catch (ArgumentException ex)
{
Console.WriteLine($"Error de negocio: {ex.Message}");
}
}
}