using System;
using System.Collections.Generic;

namespace WebApplication2;

public partial class Experience
{
    public int Id { get; set; }

    public int? FkSotrudnik { get; set; }

    public string? ObchyiDay { get; set; }

    public string? ObchyiMonth { get; set; }

    public string? ObchyiYear { get; set; }

    public string? NepreryvniyDay { get; set; }

    public string? NepreryvniyMonth { get; set; }

    public string? NepreryvniyYear { get; set; }

    public string? VislugaletDay { get; set; }

    public string? VislugaletMonth { get; set; }

    public string? VislugaletYear { get; set; }

    public DateTime? Stajrabotyposostoyaniyna { get; set; }

    public string? Del { get; set; }
}
