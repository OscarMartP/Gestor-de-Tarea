using System.Reflection;
using GestorTareas.Domain.Entities;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GestorTareas.Tests;

public class TareaTests
{
    [Test]
    public void EstaVencida_ConFechaFutura_RetornaFalse()
    {
        var tarea = new Tarea("Pagar servicio", DateTime.Today.AddDays(1), PrioridadTarea.Media);

        bool resultado = tarea.EstaVencida;

        ClassicAssert.AreEqual(false, resultado);
    }

    [TestCase(-1, true)]
    [TestCase(0, false)]
    [TestCase(2, false)]
    public void EstaVencida_ConFechasDistintas_RetornaResultadoEsperado(int diasOffset, bool esperado)
    {
        var tarea = new Tarea("Enviar reporte", DateTime.Today, PrioridadTarea.Alta);
        AsignarFechaLimiteInterna(tarea, DateTime.Today.AddDays(diasOffset));

        bool resultado = tarea.EstaVencida;

        ClassicAssert.AreEqual(esperado, resultado);
    }

    [Test]
    public void ConstructorValido_CreaTareaConPropiedadesEsperadas()
    {
        var titulo = "Comprar materiales";
        var fechaLimite = DateTime.Today.AddDays(7);
        var prioridad = PrioridadTarea.Alta;
        var descripcion = "Materiales de oficina";

        var tarea = new Tarea(titulo, fechaLimite, prioridad, descripcion);

        Assert.That(tarea.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(tarea.Titulo, Is.EqualTo(titulo));
        Assert.That(tarea.Descripcion, Is.EqualTo(descripcion));
        Assert.That(tarea.FechaLimite, Is.EqualTo(fechaLimite.Date));
        Assert.That(tarea.Prioridad, Is.EqualTo(prioridad));
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Pendiente));
    }

    [Test]
    public void Iniciar_DesdePendiente_RetornaTrueYActualizaEstado()
    {
        var tarea = new Tarea("Revisar informe", DateTime.Today.AddDays(2), PrioridadTarea.Media);

        bool resultado = tarea.Iniciar();

        Assert.That(resultado, Is.True);
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.EnProgreso));
    }

    [Test]
    public void Completar_DesdePendiente_RetornaTrueYActualizaEstado()
    {
        var tarea = new Tarea("Enviar factura", DateTime.Today.AddDays(3), PrioridadTarea.Baja);

        bool resultado = tarea.Completar();

        Assert.That(resultado, Is.True);
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Completada));
    }

    [Test]
    public void Completar_DesdeEnProgreso_RetornaTrueYActualizaEstado()
    {
        var tarea = new Tarea("Instalar actualizacion", DateTime.Today.AddDays(4), PrioridadTarea.Media);
        tarea.Iniciar();

        bool resultado = tarea.Completar();

        Assert.That(resultado, Is.True);
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Completada));
    }

    [Test]
    public void Cancelar_DesdePendiente_RetornaTrueYGuardaMotivo()
    {
        var tarea = new Tarea("Reservar sala", DateTime.Today.AddDays(5), PrioridadTarea.Media);

        bool resultado = tarea.Cancelar("No hay disponibilidad");

        Assert.That(resultado, Is.True);
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Cancelada));
        Assert.That(tarea.MotivoCancelacion, Is.EqualTo("No hay disponibilidad"));
    }

    [Test]
    public void Actualizar_DesdePendiente_RetornaTrueYActualizaCampos()
    {
        var tarea = new Tarea("Limpiar cocina", DateTime.Today.AddDays(6), PrioridadTarea.Baja);

        bool resultado = tarea.Actualizar("Limpiar cocina y baño", DateTime.Today.AddDays(8), PrioridadTarea.Media, "Incluye desinfección");

        Assert.That(resultado, Is.True);
        Assert.That(tarea.Titulo, Is.EqualTo("Limpiar cocina y baño"));
        Assert.That(tarea.Prioridad, Is.EqualTo(PrioridadTarea.Media));
        Assert.That(tarea.Descripcion, Is.EqualTo("Incluye desinfección"));
    }

    [Test]
    public void Actualizar_DesdeEnProgreso_RetornaTrueYActualizaCampos()
    {
        var tarea = new Tarea("Redactar contrato", DateTime.Today.AddDays(7), PrioridadTarea.Alta);
        tarea.Iniciar();

        bool resultado = tarea.Actualizar("Redactar contrato definitivo", DateTime.Today.AddDays(9), PrioridadTarea.Alta);

        Assert.That(resultado, Is.True);
        Assert.That(tarea.Titulo, Is.EqualTo("Redactar contrato definitivo"));
        Assert.That(tarea.FechaLimite, Is.EqualTo(DateTime.Today.AddDays(9)));
    }

    [Test]
    public void DiasRestantes_ConFechaLimiteFutura_CalculaCorrectamente()
    {
        var tarea = new Tarea("Planificar reunión", DateTime.Today.AddDays(10), PrioridadTarea.Media);

        Assert.That(tarea.DiasRestantes, Is.EqualTo(10));
    }

    [Test]
    public void Cancelar_MotivoVacio_AsignaSinEspecificar()
    {
        var tarea = new Tarea("Enviar paquete", DateTime.Today.AddDays(3), PrioridadTarea.Baja);

        bool resultado = tarea.Cancelar(string.Empty);

        Assert.That(resultado, Is.True);
        Assert.That(tarea.MotivoCancelacion, Is.EqualTo("Sin especificar"));
    }

    [Test]
    public void Constructor_TituloVacio_LanzaArgumentException()
    {
        Assert.That(() => new Tarea(string.Empty, DateTime.Today.AddDays(1), PrioridadTarea.Media), Throws.ArgumentException);
    }

    [Test]
    public void Constructor_FechaPasada_LanzaArgumentException()
    {
        Assert.That(() => new Tarea("Actualizar docs", DateTime.Today.AddDays(-1), PrioridadTarea.Baja), Throws.ArgumentException);
    }

    [TestCase(EstadoTarea.EnProgreso)]
    [TestCase(EstadoTarea.Completada)]
    [TestCase(EstadoTarea.Cancelada)]
    public void Iniciar_DesdeEstadoNoPendiente_RetornaFalse(EstadoTarea estadoInicial)
    {
        var tarea = new Tarea("Revisar plan", DateTime.Today.AddDays(2), PrioridadTarea.Media);
        if (estadoInicial == EstadoTarea.EnProgreso) tarea.Iniciar();
        if (estadoInicial == EstadoTarea.Completada) tarea.Completar();
        if (estadoInicial == EstadoTarea.Cancelada) tarea.Cancelar("Cancelada");

        bool resultado = tarea.Iniciar();

        Assert.That(resultado, Is.False);
    }

    [TestCase(EstadoTarea.Completada)]
    [TestCase(EstadoTarea.Cancelada)]
    public void Completar_DesdeEstadoFinal_RetornaFalse(EstadoTarea estadoInicial)
    {
        var tarea = new Tarea("Reunión mensual", DateTime.Today.AddDays(2), PrioridadTarea.Baja);
        if (estadoInicial == EstadoTarea.Completada) tarea.Completar();
        if (estadoInicial == EstadoTarea.Cancelada) tarea.Cancelar("Cambio de prioridad");

        bool resultado = tarea.Completar();

        Assert.That(resultado, Is.False);
    }

    [Test]
    public void Cancelar_DesdeCancelada_RetornaFalse()
    {
        var tarea = new Tarea("Pagar alquiler", DateTime.Today.AddDays(1), PrioridadTarea.Alta);
        tarea.Cancelar("Duplicada");

        bool resultado = tarea.Cancelar("Otro motivo");

        Assert.That(resultado, Is.False);
    }

    [TestCase(EstadoTarea.Completada)]
    [TestCase(EstadoTarea.Cancelada)]
    public void Actualizar_DesdeEstadoFinal_RetornaFalse(EstadoTarea estadoInicial)
    {
        var tarea = new Tarea("Enviar email", DateTime.Today.AddDays(2), PrioridadTarea.Media);
        if (estadoInicial == EstadoTarea.Completada) tarea.Completar();
        if (estadoInicial == EstadoTarea.Cancelada) tarea.Cancelar("No requiere");

        bool resultado = tarea.Actualizar("Enviar email urgente", DateTime.Today.AddDays(3), PrioridadTarea.Alta);

        Assert.That(resultado, Is.False);
    }

    
    private static void AsignarFechaLimiteInterna(Tarea tarea, DateTime fecha)
    {
        var campo = typeof(Tarea).GetField("<FechaLimite>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);

        campo.SetValue(tarea, fecha.Date);
    }
}
