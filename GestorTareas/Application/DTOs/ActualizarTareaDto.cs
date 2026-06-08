using GestorTareas.Domain.Entities;

namespace GestorTareas.Application.DTOs;

public record ActualizarTareaDto(
    string Titulo,
    string? Descripcion,
    DateTime FechaLimite,
    PrioridadTarea Prioridad);
