using GestorTareas.Application.DTOs;
using GestorTareas.Domain.Entities;
using GestorTareas.Infrastructure.Repositories;

namespace GestorTareas.Application.Services;

public class TareaService : ITareaService
{
    private readonly ITareaRepositorio repositorio;

    public TareaService(ITareaRepositorio repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<IEnumerable<TareaDto>> ObtenerTodasAsync(PrioridadTarea? prioridad = null, EstadoTarea? estado = null)
    {
        var tareas = await repositorio.ObtenerTodasAsync(prioridad, estado);
        return tareas.Select(MapDto);
    }

    public async Task<TareaDto?> ObtenerPorIdAsync(Guid id)
    {
        var tarea = await repositorio.ObtenerPorIdAsync(id);
        return tarea is null ? null : MapDto(tarea);
    }

    public async Task<TareaDto> CrearAsync(CrearTareaDto dto)
    {
        var tarea = new Tarea(dto.Titulo, dto.FechaLimite, dto.Prioridad, dto.Descripcion);

        int usuarioId = await repositorio.ObtenerUsuarioDefaultIdAsync();
        tarea.AsignarUsuario(usuarioId);

        var creada = await repositorio.CrearAsync(tarea);
        return MapDto(creada);
    }

    public async Task<TareaDto?> ActualizarAsync(Guid id, ActualizarTareaDto dto)
    {
        var tarea = await repositorio.ObtenerPorIdAsync(id);
        if (tarea is null) return null;

        bool actualizado = tarea.Actualizar(dto.Titulo, dto.FechaLimite, dto.Prioridad, dto.Descripcion);
        if (!actualizado) return null;

        var actualizada = await repositorio.ActualizarAsync(tarea);
        return actualizada is null ? null : MapDto(actualizada);
    }

    public async Task<bool> EliminarAsync(Guid id) =>
        await repositorio.EliminarAsync(id);

    public async Task<bool> CambiarEstadoAsync(Guid id, CambiarEstadoDto dto)
    {
        var tarea = await repositorio.ObtenerPorIdAsync(id);
        if (tarea is null) return false;

        bool resultado = dto.Accion.ToLowerInvariant() switch
        {
            "iniciar"   => tarea.Iniciar(),
            "completar" => tarea.Completar(),
            "cancelar"  => tarea.Cancelar(dto.MotivoCancelacion ?? string.Empty),
            _           => false
        };

        if (!resultado) return false;

        await repositorio.ActualizarAsync(tarea);
        return true;
    }

    private static TareaDto MapDto(Tarea t) => new(
        t.Id,
        t.Titulo,
        t.Descripcion,
        t.FechaCreacion,
        t.FechaLimite,
        t.Prioridad.ToString(),
        t.Estado.ToString(),
        t.EstaVencida,
        t.DiasRestantes,
        t.MotivoCancelacion);
}
