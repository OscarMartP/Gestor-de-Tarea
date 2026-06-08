using GestorTareas.Domain.Entities;

namespace GestorTareas.Application.DTOs;

public record CrearTareaDto(
    string Titulo,
    string? Descripcion,
    DateTime FechaLimite,
    PrioridadTarea Prioridad);
