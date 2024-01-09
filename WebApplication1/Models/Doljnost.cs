using System;
using System.Collections.Generic;

namespace WebApplication2;

public partial class Doljnost
{
    public int Id { get; set; }

    public int? FkSotrudnik { get; set; }

    public string? NaimenoviyDoljnosti { get; set; }

    public string? SKogo { get; set; }

    public string? PoKokoe { get; set; }

    public int? KolVo { get; set; }

    public string? Otvetstveniy { get; set; }
}
