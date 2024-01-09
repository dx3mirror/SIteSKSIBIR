using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2;

public partial class Sotrudnik
{
    public int Id { get; set; }

    public string? Lastname { get; set; }

    public string? Firstname { get; set; }

    public string? Patranomic { get; set; }

    public string? Adress { get; set; }

    public string? MestoRojd { get; set; }

    [Display(Name = "Дата рождения")]
    [DataType(DataType.Date)]
    public DateTime? Datarojdeniy { get; set; }

    public string? InYz { get; set; }

    public string? Brak { get; set; }

    public string? Del { get; set; }

    public string? IdentityNomer { get; set; }

    public string? Okin { get; set; }

    public byte[]? Avatar { get; set; }

    public virtual ICollection<Passport> Passports { get; set; } = new List<Passport>();
}
