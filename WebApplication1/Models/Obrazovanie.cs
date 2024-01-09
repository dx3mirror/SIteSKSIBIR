using System;
using System.Collections.Generic;

namespace WebApplication2;

public partial class Obrazovanie
{
    public int Id { get; set; }

    public int? FkSotrudnik { get; set; }

    public string? Obrazovanie1 { get; set; }

    public string? Nazvanieuchrejdeniya { get; set; }

    public string? KvalifikachiyapoObrazovaniyu { get; set; }

    public string? Nazvanieuchrejdeniya2 { get; set; }

    public string? KvalifikachiyapoObrazovaniyu2 { get; set; }

    public string? Poslevuzovoe { get; set; }

    public string? ProfessiaOsnova { get; set; }

    public string? ProfessiaDopolnitel { get; set; }
}
