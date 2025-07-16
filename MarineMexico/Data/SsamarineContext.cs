using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MarineMexico.Data;

public partial class SsamarineContext : DbContext
{
    public SsamarineContext()
    {
    }

    public SsamarineContext(DbContextOptions<SsamarineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<MotivosMovimientoInventario> MotivosMovimientoInventarios { get; set; }

    public virtual DbSet<MovimientosInventario> MovimientosInventarios { get; set; }

    public virtual DbSet<Talla> Tallas { get; set; }

    public virtual DbSet<TiposEmpleado> TiposEmpleados { get; set; }

    public virtual DbSet<TiposMovimientoInventario> TiposMovimientoInventarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ssamarine;Integrated Security=True;Multiple Active Result Sets=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.TipoEmpleado).WithMany(p => p.Articulos)
                .HasForeignKey(d => d.TipoEmpleadoId)
                .HasConstraintName("FK_Articulos_TiposEmpleados");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado);

            entity.ToTable("Empleado");

            entity.Property(e => e.IdEmpleado).HasColumnName("Id_Empleado");
            entity.Property(e => e.IdGrupo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("id_grupo");
            entity.Property(e => e.NombreEmpleado)
                .HasMaxLength(100)
                .HasColumnName("Nombre_Empleado");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.IdGrupo);

            entity.Property(e => e.IdGrupo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("id_grupo");
            entity.Property(e => e.Grupo1)
                .HasMaxLength(100)
                .HasColumnName("grupo");
            entity.Property(e => e.IdTipo).HasColumnName("ID_TIPO");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grupos_TiposEmpleados");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.ToTable("Inventario");

            entity.HasOne(d => d.Articulo).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.ArticuloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Articulos");

            entity.HasOne(d => d.Talla).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.TallaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Tallas");
        });

        modelBuilder.Entity<MotivosMovimientoInventario>(entity =>
        {
            entity.ToTable("MotivosMovimientoInventario");

            entity.Property(e => e.Descripcion).HasMaxLength(50);

            entity.HasOne(d => d.TipoMovimientoInventario).WithMany(p => p.MotivosMovimientoInventarios)
                .HasForeignKey(d => d.TipoMovimientoInventarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MotivosMovimientoInventario_TiposMovimientoInventario");
        });

        modelBuilder.Entity<MovimientosInventario>(entity =>
        {
            entity.ToTable("MovimientosInventario");

            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Empleado).WithMany(p => p.MovimientosInventarios)
                .HasForeignKey(d => d.EmpleadoId)
                .HasConstraintName("FK__Movimient__Emple__68487DD7");

            entity.HasOne(d => d.Inventario).WithMany(p => p.MovimientosInventarios)
                .HasForeignKey(d => d.InventarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientosInventario_Inventario");

            entity.HasOne(d => d.TipoMovimiento).WithMany(p => p.MovimientosInventarios)
                .HasForeignKey(d => d.TipoMovimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientosInventario_TiposMovimientoInventario");
        });

        modelBuilder.Entity<Talla>(entity =>
        {
            entity.Property(e => e.Notacion).HasMaxLength(10);
            entity.Property(e => e.Talla1)
                .HasMaxLength(10)
                .HasColumnName("Talla");
        });

        modelBuilder.Entity<TiposEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdTipo);

            entity.Property(e => e.IdTipo)
                .ValueGeneratedNever()
                .HasColumnName("ID_TIPO");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .HasColumnName("TIPO");
        });

        modelBuilder.Entity<TiposMovimientoInventario>(entity =>
        {
            entity.ToTable("TiposMovimientoInventario");

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
