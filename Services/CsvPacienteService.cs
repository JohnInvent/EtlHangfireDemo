using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using EtlHangfireDemo.Models;
using System.Formats.Asn1;

public class CsvPacienteService
{
    private readonly string _filePath;

    public CsvPacienteService(string filePath)
    {
        _filePath = filePath;
    }

    public List<Paciente> LeerPacientes()
    {
        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });

        return csv.GetRecords<Paciente>().ToList();
    }
}