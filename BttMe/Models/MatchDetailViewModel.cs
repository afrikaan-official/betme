namespace BttMe.Data;

public class MatchDetailViewModel
{
    public bool IsValid { get; set; }
    public int MatchId { get; set; }
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public string GameDate { get; set; }

    public string HomeIcon { get; set; }
    public string AwayIcon { get; set; }
    public List<Odd> FirstHalfGoals { get; set; }
    public List<Odd> MostGoalHalf { get; set; }
    public double FirstHalfDiff { get; set; }
    public double MostGoalHalfDiff { get; set; }
}