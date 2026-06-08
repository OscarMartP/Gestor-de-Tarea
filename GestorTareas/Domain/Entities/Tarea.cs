namespace GestorTareas.Domain.Entities;

public enum PrioridadTarea { Baja, Media, Alta }
public enum EstadoTarea { Pendiente, EnProgreso, Completada, Cancelada }

public class Tarea
{
    public Guid Id { get; private set; }
    public int UsuarioId { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaLimite { get; private set; }
    public PrioridadTarea Prioridad { get; private set; }
    public EstadoTarea Estado { get; private set; }
    public string? MotivoCancelacion { get; private set; }

   
    protected Tarea() { }

    public Tarea(string titulo, DateTime fechaLimite,
        PrioridadTarea prioridad, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El titulo es obligatorio", nameof(titulo));

        if (fechaLimite < DateTime.Now.Date)
            throw new ArgumentException("La fecha límite no puede ser pasada", nameof(fechaLimite));

        Id = Guid.NewGuid();
        Titulo = titulo.Trim();
        Descripcion = descripcion?.Trim() ?? string.Empty;
        FechaCreacion = DateTime.Now;
        FechaLimite = fechaLimite.Date;
        Prioridad = prioridad;
        Estado = EstadoTarea.Pendiente;
    }

    public void AsignarUsuario(int usuarioId) => UsuarioId = usuarioId;

    public int DiasRestantes => (FechaLimite - DateTime.Today).Days;

    public bool EstaVencida =>
        Estado != EstadoTarea.Completada &&
        Estado != EstadoTarea.Cancelada &&
        DateTime.Now.Date > FechaLimite;

    public bool Iniciar()
    {
        if (Estado != EstadoTarea.Pendiente) return false;
        Estado = EstadoTarea.EnProgreso;
        return true;
    }

    public bool Completar()
    {
        if (Estado == EstadoTarea.Completada || Estado == EstadoTarea.Cancelada) return false;
        Estado = EstadoTarea.Completada;
        return true;
    }

    public bool Cancelar(string motivo)
    {
        if (Estado == EstadoTarea.Cancelada) return false;
        Estado = EstadoTarea.Cancelada;
        MotivoCancelacion = string.IsNullOrWhiteSpace(motivo) ? "Sin especificar" : motivo;
        return true;
    }

    public bool Actualizar(string titulo, DateTime fechaLimite,
        PrioridadTarea prioridad, string? descripcion = null)
    {
        if (Estado == EstadoTarea.Completada || Estado == EstadoTarea.Cancelada)
            return false;

        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El titulo es obligatorio", nameof(titulo));

        if (fechaLimite < DateTime.Now.Date)
            throw new ArgumentException("La fecha limite no puede ser pasada", nameof(fechaLimite));

        Titulo = titulo.Trim();
        FechaLimite = fechaLimite.Date;
        Prioridad = prioridad;
        Descripcion = descripcion?.Trim() ?? string.Empty;
        return true;
    }
}
