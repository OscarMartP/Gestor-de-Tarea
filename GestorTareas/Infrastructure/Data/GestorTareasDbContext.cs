using GestorTareas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Infrastructure.Data;

public class GestorTareasDbContext : DbContext
{
    public GestorTareasDbContext(DbContextOptions<GestorTareasDbContext> options)
        : base(options) { }

    public DbSet<Tarea> Tareas => Set<Tarea>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.ToTable("Tarea");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id)
                  .HasColumnName("TareaId");
            entity.Property(t => t.Titulo)
                  .HasMaxLength(150)
                  .IsRequired();
            entity.Property(t => t.Descripcion)
                  .HasMaxLength(500);
            entity.Property(t => t.MotivoCancelacion)
                  .HasMaxLength(300);
            entity.Property(t => t.Prioridad)
                  .HasColumnType("tinyint");
            entity.Property(t => t.Estado)
                  .HasColumnType("tinyint");
            entity.Property(t => t.FechaLimite)
                  .HasColumnType("date");
            entity.Property(t => t.FechaCreacion)
                  .HasColumnType("datetime2");

            entity.HasOne<Usuario>()
                  .WithMany()
                  .HasForeignKey(t => t.UsuarioId)
                  .HasConstraintName("FK_Tarea_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                  .HasColumnName("UsuarioId")
                  .UseIdentityColumn();
            entity.Property(u => u.Nombre)
                  .HasMaxLength(80)
                  .IsRequired();
            entity.Property(u => u.Email)
                  .HasMaxLength(120)
                  .IsRequired();
            entity.HasIndex(u => u.Email)
                  .IsUnique();
            entity.Property(u => u.PasswordHash)
                  .HasMaxLength(255)
                  .IsRequired();
            entity.Property(u => u.FechaRegistro)
                  .HasColumnType("datetime2");
        });
    }
}
