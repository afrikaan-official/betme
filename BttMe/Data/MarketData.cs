namespace BttMe.Data;

public class MarketData
{
    public Dictionary<string, List<EventMarket>> EventMarketIds { get; set; }
}

public class EventMarket
{
    public long MarketId { get; set; }
    public int MarketSubType { get; set; }
    public int MarketType { get; set; }
    public string MarketName { get; set; }
}