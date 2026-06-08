using GestorTareas.Domain.Entities;

namespace GestorTareas.Infrastructure.Repositories;

public interface ITareaRepositorio
{
    Task<IEnumerable<Tarea>> ObtenerTodasAsync(PrioridadTarea? prioridad = null, EstadoTarea? estado = null);
    Task<Tarea?> ObtenerPorIdAsync(Guid id);
    Task<Tarea> CrearAsync(Tarea tarea);
    Task<Tarea?> ActualizarAsync(Tarea tarea);
    Task<bool> EliminarAsync(Guid id);
    Task<int> ObtenerUsuarioDefaultIdAsync();
}
