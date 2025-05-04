namespace BttMe.Data;

public class MarketGroup
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<int> Counts { get; set; }
    public List<Odd> Odds { get; set; }
}