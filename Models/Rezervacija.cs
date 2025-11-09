using System.ComponentModel.DataAnnotations;

namespace AutoServis.Models;

public enum StatusServisa
{
    Cakajoce, VTehu, Koncano, Stornirano
}

public class Rezervacija
{
    public int RezervacijaID { get; set; }
    public int VoziloID { get; set; }
    public int StrankaID { get; set; }
    public int? MehanikID { get; set; } 
    
    public DateTime DatumRezervacije { get; set; }
    public string? OpisTezave { get; set; }
    public StatusServisa? Status { get; set; }
    
    public string? Diagnoza { get; set; }
    public string? OpravljenaDela { get; set; }
    public decimal? Cena { get; set; }
    public bool Potrjeno { get; set; } = false;
    public DateTime? DatumZacetka { get; set; }
    public DateTime? DatumKonca { get; set; }
    
    public Vozilo? Vozilo { get; set; }
    public Stranka? Stranka { get; set; }
    public Mehanik? Mehanik { get; set; }
}