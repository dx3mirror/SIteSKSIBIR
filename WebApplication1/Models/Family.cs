using System;
using System.Collections.Generic;

namespace WebApplication2;

public partial class Family
{
    public int Id { get; set; }

    public int? FkSotrudnik { get; set; }

    public string? Fio { get; set; }

    public string? StepenRodstva { get; set; }

    public DateTime? DataRojdeniya { get; set; }
}
