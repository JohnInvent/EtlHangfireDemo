using System.Text.RegularExpressions;
using EtlHangfireDemo.Data;
using EtlHangfireDemo.Models;

public class PacienteJob
{
    private readonly CsvPacienteService _csvService;
    private readonly ApplicationDbContext _db;

    public PacienteJob(CsvPacienteService csvService, ApplicationDbContext db)
    {
        _csvService = csvService;
        _db = db;
    }

    public void Ejecutar()
    {
        var pacientesCsv = _csvService.LeerPacientes();

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        var emailsExistentes = _db.Pacientes.Select(p => p.Email).ToHashSet();

        var pacientesValidos = new List<Paciente>();
        var errores = new List<PacienteErrorLog>();

        foreach (var p in pacientesCsv)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre))
            {
                errores.Add(new PacienteErrorLog { Email = p.Email, Motivo = "Nombre vacío" });
                continue;
            }
          

            if (!emailRegex.IsMatch(p.Email))
            {
                errores.Add(new PacienteErrorLog { Email = p.Email, Motivo = "Email inválido" });
                continue;
            }

            if (emailsExistentes.Contains(p.Email))
            {
                errores.Add(new PacienteErrorLog { Email = p.Email, Motivo = "Email duplicado" });
                continue;
            }

            pacientesValidos.Add(p);
        }

        if (pacientesValidos.Any())
        {
            _db.Pacientes.AddRange(pacientesValidos);
            Console.WriteLine($"Insertados {pacientesValidos.Count} pacientes válidos.");
        }

        if (errores.Any())
        {
            _db.PacienteErrores.AddRange(errores);
            Console.WriteLine($"Registrados {errores.Count} errores en auditoría.");
        }

        _db.SaveChanges();
    }
}