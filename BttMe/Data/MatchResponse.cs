namespace BttMe.Data;

public class Match
{
    public int MatchID { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string DateTime { get; set; }
    public string LeagueCode { get; set; }
    public string LeagueFlag { get; set; }
    public string Country { get; set; }
    public string League { get; set; }
    public string Teams { get; set; }
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public string Team1Logo { get; set; }
    public string Team2Logo { get; set; }
    public int MatchResult { get; set; }
    public int MB { get; set; }
    public int Result { get; set; }
    public int GameResult { get; set; }
    public int LiveStatus { get; set; }
    public int BetCount { get; set; }
    public double? HomeWin { get; set; }
    public double? Draw { get; set; }
    public double? AwayWin { get; set; }
    public double? Under25 { get; set; }
    public double? Over25 { get; set; }
}

public class MatchResponse
{
    public string status { get; set; }
    public string message { get; set; }
    public string messageTR { get; set; }
    public int systemTime { get; set; }
    public string endpoint { get; set; }
    public int rowCount { get; set; }
    public int creditUsed { get; set; }
    public List<Match> data { get; set; }
}