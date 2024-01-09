using System;
using System.Collections.Generic;

namespace WebApplication2;

public partial class Otpusk
{
    public int Id { get; set; }

    public int? FkSotrudnik { get; set; }

    public string? VidOtpuska { get; set; }

    public DateTime? PeriodS { get; set; }

    public DateTime? PeriodPo { get; set; }

    public string? Day { get; set; }

    public DateTime? SKakogo { get; set; }

    public DateTime? PoKakoe { get; set; }

    public string? Prichina { get; set; }
}
