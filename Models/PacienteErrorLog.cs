public class PacienteErrorLog
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Motivo { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}