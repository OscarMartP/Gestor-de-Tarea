CREATE DATABASE GestorTareasSimple;
GO

USE GestorTareasSimple;
GO

CREATE TABLE Usuario (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Email NVARCHAR(120) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

CREATE TABLE Tarea (
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
        FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),

    CONSTRAINT CHK_Tarea_Prioridad
        CHECK (Prioridad IN (0,1,2)),

    CONSTRAINT CHK_Tarea_Estado
        CHECK (Estado IN (0,1,2,3)),

    CONSTRAINT CHK_Tarea_Titulo
        CHECK (LEN(LTRIM(RTRIM(Titulo))) > 0)
);
GO

CREATE INDEX IX_Tarea_Usuario ON Tarea(UsuarioId);
CREATE INDEX IX_Tarea_Usuario_Estado ON Tarea(UsuarioId, Estado);
CREATE INDEX IX_Tarea_Usuario_FechaLimite ON Tarea(UsuarioId, FechaLimite);
GO

-- Datos de ejemplo opcionales
INSERT INTO Usuario (Nombre, Email, PasswordHash)
VALUES
('Oscar', 'oscar@demo.com', 'HASH_DE_EJEMPLO');
GO

DECLARE @UsuarioId INT = (SELECT TOP 1 UsuarioId FROM Usuario ORDER BY UsuarioId);

INSERT INTO Tarea (UsuarioId, Titulo, Descripcion, FechaLimite, Prioridad, Estado)
VALUES
(@UsuarioId, 'Preparar entrega', 'Subir avance del proyecto', DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), 2, 0),
(@UsuarioId, 'Estudiar SQL', 'Repasar joins e indices', DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), 1, 1),
(@UsuarioId, 'Actualizar README', 'Documentar cambios recientes', DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 0, 0);
GO


