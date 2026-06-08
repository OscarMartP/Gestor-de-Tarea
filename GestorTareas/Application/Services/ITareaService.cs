using GestorTareas.Application.DTOs;
using GestorTareas.Domain.Entities;

namespace GestorTareas.Application.Services;

public interface ITareaService
{
    Task<IEnumerable<TareaDto>> ObtenerTodasAsync(PrioridadTarea? prioridad = null, EstadoTarea? estado = null);
    Task<TareaDto?> ObtenerPorIdAsync(Guid id);
    Task<TareaDto> CrearAsync(CrearTareaDto dto);
    Task<TareaDto?> ActualizarAsync(Guid id, ActualizarTareaDto dto);
    Task<bool> EliminarAsync(Guid id);
    Task<bool> CambiarEstadoAsync(Guid id, CambiarEstadoDto dto);
}
