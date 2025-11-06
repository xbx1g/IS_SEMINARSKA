using System;
using System.Collections.Generic;

namespace AutoServis.Models;
public class Stranka
{
    public int ID { get; set; }
    public string? Priimek { get; set; }
    public string? Ime { get; set; }
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public string? Naslov { get; set; }  // DODANO
    public DateTime DatumRegistracije { get; set; }

    public ICollection<Rezervacija>? Rezervacije { get; set; }
}