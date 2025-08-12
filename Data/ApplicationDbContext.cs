using Microsoft.EntityFrameworkCore;
using EtlHangfireDemo.Models;

namespace EtlHangfireDemo.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<PacienteErrorLog> PacienteErrores { get; set; }
}