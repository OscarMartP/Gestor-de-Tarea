namespace GestorTareas.Application.DTOs;

public record TareaDto(
    Guid Id,
    string Titulo,
    string Descripcion,
    DateTime FechaCreacion,
    DateTime FechaLimite,
    string Prioridad,
    string Estado,
    bool EstaVencida,
    int DiasRestantes,
    string? MotivoCancelacion);
