namespace GestorTareas.Application.DTOs;

/// <param name="Accion">iniciar | completar | cancelar</param>
/// <param name="MotivoCancelacion">Obligatorio si Accion es 'cancelar'.</param>
public record CambiarEstadoDto(string Accion, string? MotivoCancelacion);
