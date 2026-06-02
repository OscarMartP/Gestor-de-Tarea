# Docker setup para Gestor de Tareas

## 1) Configurar variables
1. Copia `.env.example` a `.env`.
2. Cambia `MSSQL_SA_PASSWORD` por una clave fuerte.
3. Si cambias la clave, actualiza tambien `GESTOR_TAREAS_DB_CONNECTION`.

## 2) Levantar servicios
Desde la raiz del proyecto:

```powershell
docker compose up -d
```

Servicio creado:
- SQL Server: `localhost:14333`

## 3) Credenciales SQL
- Usuario: `sa`
- Password: el valor de `MSSQL_SA_PASSWORD`
- Base: `GestorTareasSimple`

## 4) Crear conexion en DBeaver Desktop
1. Abre DBeaver y selecciona `New Database Connection`.
2. Elige `SQL Server` y pulsa `Next`.
3. Completa los datos:
	- Host: `localhost`
	- Port: `14333`
	- Database: `GestorTareasSimple`
	- Username: `sa`
	- Password: el valor de `MSSQL_SA_PASSWORD`
4. En `Driver properties`, si aparece validacion SSL, deja activado `trustServerCertificate=true`.
5. Pulsa `Test Connection` y luego `Finish`.

## 5) Ejecutar la app usando la cadena del .env
En PowerShell (desde la raiz):

```powershell
$env:GESTOR_TAREAS_DB_CONNECTION = "Server=localhost,14333;Database=GestorTareasSimple;User Id=sa;Password=TuPassword123!;Encrypt=False;TrustServerCertificate=True;"
dotnet run --project GestorTareas
```

## 6) Apagar servicios
```powershell
docker compose down
```

Si quieres eliminar tambien los datos persistidos del volumen:

```powershell
docker compose down -v
```
