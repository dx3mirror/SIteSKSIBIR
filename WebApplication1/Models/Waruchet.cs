using System;
using System.Collections.Generic;

namespace WebApplication2;

public partial class Waruchet
{
    public int Id { get; set; }

    public int? FkSotrudnik { get; set; }

    public string? Kategorya { get; set; }

    public string? Zvanie { get; set; }

    public string? Sostav { get; set; }

    public string? KodVus { get; set; }

    public string? SostoyanieVoinskogo { get; set; }

    public string? NazvanieKomisariata { get; set; }

    public string? Kategoria { get; set; }
}
