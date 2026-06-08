using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using GestorTareas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ITareaService service;

    public TareasController(ITareaService service)
    {
        this.service = service;
    }

    // GET api/tareas
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? prioridad = null, [FromQuery] string? estado = null)
    {
        PrioridadTarea? pFilter = null;
        EstadoTarea? eFilter = null;

        if (!string.IsNullOrWhiteSpace(prioridad))
        {
            if (!Enum.TryParse<PrioridadTarea>(prioridad, true, out var pVal))
                return BadRequest("Valor de 'prioridad' inválido. Valores válidos: Baja, Media, Alta.");
            pFilter = pVal;
        }

        if (!string.IsNullOrWhiteSpace(estado))
        {
            if (!Enum.TryParse<EstadoTarea>(estado, true, out var eVal))
                return BadRequest("Valor de 'estado' inválido. Valores válidos: Pendiente, EnProgreso, Completada, Cancelada.");
            eFilter = eVal;
        }

        var tareas = await service.ObtenerTodasAsync(pFilter, eFilter);
        return Ok(tareas);
    }

    // GET api/tareas/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var tarea = await service.ObtenerPorIdAsync(id);
        return tarea is null ? NotFound() : Ok(tarea);
    }

    // POST api/tareas
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearTareaDto dto)
    {
        var creada = await service.CrearAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
    }

    // PUT api/tareas/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarTareaDto dto)
    {
        var actualizada = await service.ActualizarAsync(id, dto);
        if (actualizada is null) return NotFound();
        return Ok(actualizada);
    }

    // DELETE api/tareas/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool eliminada = await service.EliminarAsync(id);
        return eliminada ? NoContent() : NotFound();
    }

    // PATCH api/tareas/{id}/estado
    [HttpPatch("{id:guid}/estado")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] CambiarEstadoDto dto)
    {
        bool actualizado = await service.CambiarEstadoAsync(id, dto);
        if (!actualizado) return BadRequest("No fue posible cambiar el estado. Verifica la accion y el estado actual de la tarea.");
        return NoContent();
    }
}
