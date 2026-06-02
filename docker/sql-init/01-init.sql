IF DB_ID('GestorTareasSimple') IS NULL
BEGIN
    CREATE DATABASE GestorTareasSimple;
END;
GO

USE GestorTareasSimple;
GO

IF OBJECT_ID('dbo.Usuario', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuario (
        UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(80) NOT NULL,
        Email NVARCHAR(120) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        FechaRegistro DATETIME2 NOT NULL DEFAULT SYSDATETIME()
    );
END;
GO

IF OBJECT_ID('dbo.Tarea', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tarea (
        TareaId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        UsuarioId INT NOT NULL,
        Titulo NVARCHAR(150) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        FechaCreacion DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        FechaLimite DATE NULL,
        Prioridad TINYINT NOT NULL DEFAULT 1,
        Estado TINYINT NOT NULL DEFAULT 0,
        MotivoCancelacion NVARCHAR(300) NULL,

        CONSTRAINT FK_Tarea_Usuario
            FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuario(UsuarioId),

        CONSTRAINT CHK_Tarea_Prioridad
            CHECK (Prioridad IN (0,1,2)),

        CONSTRAINT CHK_Tarea_Estado
            CHECK (Estado IN (0,1,2,3)),

        CONSTRAINT CHK_Tarea_Titulo
            CHECK (LEN(LTRIM(RTRIM(Titulo))) > 0)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Tarea_Usuario' AND object_id = OBJECT_ID('dbo.Tarea'))
BEGIN
    CREATE INDEX IX_Tarea_Usuario ON dbo.Tarea(UsuarioId);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Tarea_Usuario_Estado' AND object_id = OBJECT_ID('dbo.Tarea'))
BEGIN
    CREATE INDEX IX_Tarea_Usuario_Estado ON dbo.Tarea(UsuarioId, Estado);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Tarea_Usuario_FechaLimite' AND object_id = OBJECT_ID('dbo.Tarea'))
BEGIN
    CREATE INDEX IX_Tarea_Usuario_FechaLimite ON dbo.Tarea(UsuarioId, FechaLimite);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Email = 'oscar@demo.com')
BEGIN
    INSERT INTO dbo.Usuario (Nombre, Email, PasswordHash)
    VALUES ('Oscar', 'oscar@demo.com', 'HASH_DE_EJEMPLO');
END;
GO
