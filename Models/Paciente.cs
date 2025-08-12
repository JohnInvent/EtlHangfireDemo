using System.ComponentModel.DataAnnotations;

namespace EtlHangfireDemo.Models;

public class Paciente
{
    [Key]
    public int IdPaciente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
