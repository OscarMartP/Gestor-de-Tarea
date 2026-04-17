class Program
{
	private static readonly List<Tarea> tareas = new();

	static void Main(string[] args)
	{
		bool salir = false;

		while (!salir)
		{
			MostrarMenu();
			Console.Write("Selecciona una opcion: ");
			string? opcion = Console.ReadLine();

			Console.WriteLine();
			switch (opcion)
			{
				case "1":
					CrearTarea();
					break;
				case "2":
					ListarTareas();
					break;
				case "3":
					ActualizarTarea();
					break;
				case "4":
					EliminarTarea();
					break;
				case "5":
					CambiarEstado();
					break;
				case "0":
					salir = true;
					break;
				default:
					Console.WriteLine("Opcion no valida.\n");
					break;
			}
		}

		Console.WriteLine("Aplicacion finalizada.");
	}

	static void MostrarMenu()
	{
		Console.WriteLine("========== GESTOR DE TAREAS (CRUD) ==========");
		Console.WriteLine("1. Crear tarea");
		Console.WriteLine("2. Listar tareas");
		Console.WriteLine("3. Actualizar tarea");
		Console.WriteLine("4. Eliminar tarea");
		Console.WriteLine("5. Cambiar estado de una tarea");
		Console.WriteLine("0. Salir");
		Console.WriteLine("=============================================");
	}

	static void CrearTarea()
	{
		try
		{
			string titulo = LeerTextoObligatorio("Titulo: ");
			string descripcion = LeerTextoOpcional("Descripcion (opcional): ");
			DateTime fechaLimite = LeerFecha("Fecha limite (yyyy-MM-dd): ");
			PrioridadTarea prioridad = LeerPrioridad();

			var tarea = new Tarea(titulo, fechaLimite, prioridad, descripcion);
			tareas.Add(tarea);

			Console.WriteLine("Tarea creada correctamente.");
			Console.WriteLine($"Id: {tarea.Id}");
			Console.WriteLine();
		}
		catch (ArgumentException ex)
		{
			Console.WriteLine($"Error: {ex.Message}\n");
		}
	}

	static void ListarTareas()
	{
		if (tareas.Count == 0)
		{
			Console.WriteLine("No hay tareas registradas.\n");
			return;
		}

		Console.WriteLine("Listado de tareas:");
		foreach (var tarea in tareas)
		{
			Console.WriteLine(tarea);
		}
		Console.WriteLine();
	}

	static void ActualizarTarea()
	{
		if (!TryObtenerTareaPorId(out Tarea? tarea))
		{
			return;
		}

		try
		{
			string titulo = LeerTextoObligatorio("Nuevo titulo: ");
			string descripcion = LeerTextoOpcional("Nueva descripcion (opcional): ");
			DateTime fechaLimite = LeerFecha("Nueva fecha limite (yyyy-MM-dd): ");
			PrioridadTarea prioridad = LeerPrioridad();

			bool actualizado = tarea!.Actualizar(titulo, fechaLimite, prioridad, descripcion);
			Console.WriteLine(actualizado
				? "Tarea actualizada correctamente.\n"
				: "No se puede actualizar una tarea completada o cancelada.\n");
		}
		catch (ArgumentException ex)
		{
			Console.WriteLine($"Error: {ex.Message}\n");
		}
	}

	static void EliminarTarea()
	{
		if (!TryObtenerTareaPorId(out Tarea? tarea))
		{
			return;
		}

		tareas.Remove(tarea!);
		Console.WriteLine("Tarea eliminada correctamente.\n");
	}

	static void CambiarEstado()
	{
		if (!TryObtenerTareaPorId(out Tarea? tarea))
		{
			return;
		}

		Console.WriteLine("1. Iniciar");
		Console.WriteLine("2. Completar");
		Console.WriteLine("3. Cancelar");
		Console.Write("Selecciona accion: ");
		string? opcion = Console.ReadLine();

		bool resultado = opcion switch
		{
			"1" => tarea!.Iniciar(),
			"2" => tarea!.Completar(),
			"3" => tarea!.Cancelar(LeerTextoOpcional("Motivo de cancelacion: ")),
			_ => false
		};

		Console.WriteLine(resultado ? "Estado actualizado.\n" : "No fue posible cambiar el estado.\n");
	}

	static bool TryObtenerTareaPorId(out Tarea? tarea)
	{
		tarea = null;

		if (tareas.Count == 0)
		{
			Console.WriteLine("No hay tareas registradas.\n");
			return false;
		}

		ListarTareas();
		Console.Write("Escribe el Id de la tarea: ");
		string? inputId = Console.ReadLine();

		if (!Guid.TryParse(inputId, out Guid id))
		{
			Console.WriteLine("Id invalido.\n");
			return false;
		}

		tarea = tareas.FirstOrDefault(t => t.Id == id);
		if (tarea is null)
		{
			Console.WriteLine("No se encontro una tarea con ese Id.\n");
			return false;
		}

		return true;
	}

	static string LeerTextoObligatorio(string mensaje)
	{
		while (true)
		{
			Console.Write(mensaje);
			string? texto = Console.ReadLine();
			if (!string.IsNullOrWhiteSpace(texto))
			{
				return texto.Trim();
			}

			Console.WriteLine("El valor es obligatorio.");
		}
	}

	static string LeerTextoOpcional(string mensaje)
	{
		Console.Write(mensaje);
		return (Console.ReadLine() ?? string.Empty).Trim();
	}

	static DateTime LeerFecha(string mensaje)
	{
		while (true)
		{
			Console.Write(mensaje);
			string? entrada = Console.ReadLine();

			if (DateTime.TryParse(entrada, out DateTime fecha))
			{
				return fecha.Date;
			}

			Console.WriteLine("Fecha invalida. Usa formato yyyy-MM-dd.");
		}
	}

	static PrioridadTarea LeerPrioridad()
	{
		while (true)
		{
			Console.Write("Prioridad (0=Baja, 1=Media, 2=Alta): ");
			string? entrada = Console.ReadLine();

			if (int.TryParse(entrada, out int valor)
				&& Enum.IsDefined(typeof(PrioridadTarea), valor))
			{
				return (PrioridadTarea)valor;
			}

			Console.WriteLine("Prioridad invalida.");
		}
	}
}