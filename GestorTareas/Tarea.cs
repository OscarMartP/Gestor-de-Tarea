
public enum PrioridadTarea { Baja, Media, Alta } 
public enum EstadoTarea { Pendiente, EnProgreso, Completada, Cancelada }

public class Tarea
{
    public Guid Id { get; }
    public string Titulo { get; }
    public string Descripcion { get; }
    public DateTime FechaCreacion { get; }
    public DateTime FechaLimite { get; }
    public PrioridadTarea Prioridad { get; }

    private EstadoTarea estado;
    public EstadoTarea Estado => estado;

    private string? motivoCancelacion;

    // Constructor
    public Tarea(string titulo, DateTime fechaLimite,
        PrioridadTarea prioridad, string descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El titulo es obligatorio", nameof(titulo));

        if (fechaLimite < DateTime.Now.Date)
            throw new ArgumentException("La fecha límite no puede ser pasada", nameof(fechaLimite));

        this.Id = Guid.NewGuid();
        this.Titulo = titulo.Trim();

        if (descripcion != null)
        {
            this.Descripcion = descripcion.Trim();
        }
        else
        {
            this.Descripcion = string.Empty;
        }

        this.FechaCreacion = DateTime.Now;
        this.FechaLimite = fechaLimite.Date;
        this.Prioridad = prioridad;
        this.estado = EstadoTarea.Pendiente;
    }

    // Metodos
    public int DiasRestantes => (FechaLimite - DateTime.Today).Days;

    public bool EstaVencida =>
        estado != EstadoTarea.Completada &&
        estado != EstadoTarea.Cancelada &&
        DateTime.Now.Date > FechaLimite;

    public bool Iniciar()
    {
        if (estado != EstadoTarea.Pendiente) return false;
        estado = EstadoTarea.EnProgreso;
        return true;
    }

    public bool Completar()
    {
        if (estado == EstadoTarea.Completada ||
            estado == EstadoTarea.Cancelada) return false;

        estado = EstadoTarea.Completada;
        return true;
    }

    public bool Cancelar(string motivo)
    {
        if (estado == EstadoTarea.Cancelada) return false;

        estado = EstadoTarea.Cancelada;

        if (motivo != null)
        {
            motivoCancelacion = motivo;
        }
        else
        {
            motivoCancelacion = "Sin especificar";
        }

        return true;
    }

    public override string ToString()
    {
        string resultado = $"Tarea {Id.ToString()[..8]} | {Titulo} | " +
                           $"Limite: {FechaLimite:dd/MM/yy} | " +
                           $"Prioridad: {Prioridad} | {estado}";

        if (EstaVencida)
        {
            resultado += " [VENCIDA]";
        }

        return resultado;
    }
}