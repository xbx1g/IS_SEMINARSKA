namespace AutoServis.Models;

public class Mehanik
{
    public int MehanikID { get; set; }
    public string? Ime { get; set; }
    public string? Priimek { get; set; }
    public string? Specializacija { get; set; }
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public DateTime DatumZaposlitve { get; set; } = DateTime.Now;
    
    public ICollection<Rezervacija>? Rezervacije { get; set; }
}