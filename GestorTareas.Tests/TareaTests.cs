using System.Reflection;
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
    //HELPER TEST PARA MODIFICAR LA FECHA LIMITE DE UNA TAREA, YA QUE EL SETTER ES PRIVADO
    private static void AsignarFechaLimiteInterna(Tarea tarea, DateTime fecha)
    {
        var campo = typeof(Tarea).GetField("<FechaLimite>k__BackingField",
        BindingFlags.Instance | BindingFlags.NonPublic);

        campo.SetValue(tarea, fecha.Date);
    }

    [Test]
    public void Test_FechaLimite_Modificada()
    {
        var tarea = new Tarea("Test", DateTime.Today.AddDays(5), PrioridadTarea.Media);

        AsignarFechaLimiteInterna(tarea, DateTime.Today.AddDays(10));

        Assert.That(tarea.FechaLimite, Is.EqualTo(DateTime.Today.AddDays(10)));
    }
}
