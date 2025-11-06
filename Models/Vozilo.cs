using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServis.Models;
public class Vozilo
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int VoziloID { get; set; }
    
    public ApplicationUser? Lastnik { get; set; }

    public DateTime? DatumUstvarjen { get; set; }

    public DateTime? DatumUrejen { get; set; }

    public string? Znamka { get; set; }
    public string? Model { get; set; }
    public string? Registracija { get; set; }
    public int Letnik { get; set; }

    public ICollection<Rezervacija>? Rezervacije { get; set; }
}