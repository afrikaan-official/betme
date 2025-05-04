using BttMe.Data;

namespace BttMe.Models;

public class MatchListViewModel
{
    public List<MatchViewModel> MatchVMList { get; set; }

    public MatchListViewModel()
    {
        MatchVMList = new List<MatchViewModel>();
    }

    public void AddOdds(int matchId,double mostGoalHalf1,double mostGoalHalf2,double firstHalf1,double firstHalf2)
    {
        var match= MatchVMList.FirstOrDefault(x => x.MatchID == matchId);
        if (match != null)
        {
            match.MostGoalsHalf1 = mostGoalHalf1;
            match.MostGoalsHalf2 = mostGoalHalf2;
            match.FirstHalf1 = firstHalf1;
            match.FirstHalf2 = firstHalf2;
        }
    }
}

public class MatchViewModel
{
    public int MatchID { get; set; }
    public DateTime DateTime { get; set; }
    public string Date { get; set; }
    public string MatchName { get; set; }
    public double MostGoalsHalf1 { get; set; }
    public double MostGoalsHalf2 { get; set; }
    public double MostGoalsHalfDiff
    {
        get
        {
            return Math.Abs(MostGoalsHalf1 - MostGoalsHalf2);
        }
    }
    public double FirstHalf1 { get; set; }
    public double FirstHalf2 { get; set; }


}