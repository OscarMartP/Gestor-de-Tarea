using GestorTareas.Domain.Entities;
using GestorTareas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Infrastructure.Repositories;

public class TareaRepositorio : ITareaRepositorio
{
    private readonly GestorTareasDbContext context;

    public TareaRepositorio(GestorTareasDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Tarea>> ObtenerTodasAsync(PrioridadTarea? prioridad = null, EstadoTarea? estado = null)
    {
        var query = context.Tareas.AsNoTracking().AsQueryable();

        if (prioridad.HasValue)
            query = query.Where(t => t.Prioridad == prioridad.Value);

        if (estado.HasValue)
            query = query.Where(t => t.Estado == estado.Value);

        return await query.OrderByDescending(t => t.FechaCreacion).ToListAsync();
    }

    public async Task<Tarea?> ObtenerPorIdAsync(Guid id) =>
        await context.Tareas.FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Tarea> CrearAsync(Tarea tarea)
    {
        context.Tareas.Add(tarea);
        await context.SaveChangesAsync();
        return tarea;
    }

    public async Task<Tarea?> ActualizarAsync(Tarea tarea)
    {
        // La entidad fue obtenida con tracking; solo necesitamos guardar cambios
        await context.SaveChangesAsync();
        return tarea;
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        var tarea = await context.Tareas.FindAsync(id);
        if (tarea is null) return false;

        context.Tareas.Remove(tarea);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<int> ObtenerUsuarioDefaultIdAsync()
    {
        var usuario = await context.Usuarios.FirstOrDefaultAsync();
        return usuario?.Id ?? throw new InvalidOperationException(
            "No hay usuarios en la base de datos. Verifica que el script de inicializacion se haya ejecutado.");
    }
}
