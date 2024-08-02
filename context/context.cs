using Microsoft.EntityFrameworkCore;
using webApi.models;

namespace webApi;

public class Context:DbContext{
    public DbSet<cliente> clientes {get;set;}
    public DbSet<libro> libros {get;set;}
    public DbSet<orden> ordenes {get;set;}

    public Context(DbContextOptions<Context> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<libro>(libro =>{
            libro.ToTable("Libro");
            libro.HasKey(p=>p.IdLibro);
            libro.Property(p=>p.name).IsRequired().HasMaxLength(200);
            libro.Property(p=>p.author).HasMaxLength(50);
            libro.Property(p => p.date)
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)), // Convertir DateOnly a DateTime
                    v => DateOnly.FromDateTime(v)           // Convertir DateTime a DateOnly
                ).HasColumnType("date");     
        });

        modelBuilder.Entity<cliente>(cliente=>{
            cliente.ToTable("cliente");
            cliente.HasKey(p=>p.IdCliente);
            cliente.Property(p=>p.name).IsRequired().HasMaxLength(50);
            cliente.Property(p=>p.age).IsRequired();
            cliente.Property(p=>p.adress).IsRequired().HasMaxLength(100);
            
        });

        modelBuilder.Entity<orden>(orden=>{
            orden.ToTable("orden");
            orden.HasKey(p=>p.IdOrden);
           orden.Property(p => p.dateOrden)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        
        orden.Property(p => p.dateDevolucion)
            .IsRequired()
            .HasConversion(
                v => v.ToDateTime(new TimeOnly(0, 0)), // Convertir DateOnly a DateTime
                v => DateOnly.FromDateTime(v))          // Convertir DateTime a DateOnly
            .HasColumnType("date");
            orden.HasOne(p=>p.clientes).WithMany(p=>p.ordenes).HasForeignKey(p=>p.IdCliente).OnDelete(DeleteBehavior.Cascade);
            orden.HasOne(p=>p.libros).WithMany(p=>p.ordenes).HasForeignKey(p=>p.IdLibro).OnDelete(DeleteBehavior.Cascade);
            
        });
    }
}
