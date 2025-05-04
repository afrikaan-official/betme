namespace BttMe.Data;

public class BilyonerEvent
{
    public int Id { get; set; }
    public int CompetitionId { get; set; }
    public OtherOdd OtherOdd { get; set; }
    public List<MarketGroup> MarketGroups { get; set; }
    public bool HasParentEvent { get; set; }
    public string Slgn { get; set; }
    public DateTime Esd { get; set; }
    public long Esdl { get; set; }
    public string Lgn { get; set; }
    public int Mbs { get; set; }
    public long BrdId { get; set; }
    public int Bs { get; set; }
    public int Il { get; set; }
    public bool Him { get; set; }
    public int Bf { get; set; }
    public string Strd { get; set; }
    public string Strt { get; set; }
    public int Et { get; set; }
    public int St { get; set; }
    public long Ev { get; set; }
    public int Htpi { get; set; }
    public int Atpi { get; set; }
    public string Htn { get; set; }
    public string Atn { get; set; }
    public string En { get; set; }
    public string N { get; set; }
    public long Lgo { get; set; }
    public bool Htw { get; set; }
}



