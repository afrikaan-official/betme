using BttMe.Data;

namespace BttMe.Models;

public class MatchListViewModel
{
    public List<MatchViewModel> MatchVMList { get; set; }

    public MatchListViewModel()
    {
        MatchVMList = new List<MatchViewModel>();
    }

}

public class MatchViewModel
{
    public int MatchID { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string DateTime { get; set; }
    public string LeagueCode { get; set; }
    public string LeagueFlag { get; set; }
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public double? HomeWin { get; set; }
    public double? Draw { get; set; }
    public double? AwayWin { get; set; }
    public double? Under25 { get; set; }
    public double? Over25 { get; set; }
    public double? MostGoalsHalf1 { get; set; }
    public double? MostGoalsHalf2 { get; set; }
    public double? MostGoalsHalfDiff
    {
        get
        {
            return Math.Abs(MostGoalsHalf1.Value - MostGoalsHalf2.Value);
        }
    }
    public double? FirstHalf1 { get; set; }
    public double? FirstHalf2 { get; set; }
}