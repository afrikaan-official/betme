using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using BttMe.Data;
using Microsoft.AspNetCore.Mvc;
using BttMe.Models;

namespace BttMe.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("nosyapi");
    }

    public async Task<IActionResult> Index()
    {
        var matchData = await System.IO.File.ReadAllTextAsync(Environment.CurrentDirectory + "/Data/matches.json");
        var model = JsonSerializer.Deserialize<MatchResponse>(matchData);

        return View(model);
    }

    [HttpGet]
    public async Task<ViewResult> MatchDetail(int gameId)
    {
        var response = await _httpClient.GetAsync($"bettable-matches/details?matchID={gameId}");

        if ((int)response.StatusCode == 401)
        {
            return View("401");
        }

        var matchDetailData = await response.Content.ReadAsStringAsync();
        //var matchDetailData = await System.IO.File.ReadAllTextAsync(Environment.CurrentDirectory + "/Data/detail.json");
        var md = JsonSerializer.Deserialize<MatchDetail>(matchDetailData);
        var model = md.data[0];

        if (!model.Bets.Any(x => x.gameName == "Hangi Yarıda Daha Fazla Gol Olur") || !model.Bets.Any(x => x.gameName == "İlk Yarı Sonucu"))
        {
            return View("MatchDetail", new MatchDetailViewModel
            {
                FirstHalfGoals =[],
                MostGoalHalf = [],
                Team1 = model.Team1,
                Team2 = model.Team2,
                GameDate = model.DateTime,
                MatchId = model.MatchID,
                HomeIcon = model.Team1Logo,
                AwayIcon = model.Team2Logo,
                IsValid = false,
                FirstHalfDiff = 0,
                MostGoalHalfDiff = 0
            });
        }

        var firstHalfGoalsOdds = model.Bets
            .Where(b => b.gameName == "İlk Yarı Sonucu")
            .SelectMany(b => b.odds);

        var mostGoalsHalfOdds = model.Bets
            .Where(b => b.gameName == "Hangi Yarıda Daha Fazla Gol Olur")
            .SelectMany(b => b.odds);

        var firstHalfGoalsOdd1 = firstHalfGoalsOdds
            .FirstOrDefault(o => o.value == "1");

        var firstHalfGoalsOddX = firstHalfGoalsOdds
            .FirstOrDefault(o => o.value == "0");

        var firstHalfGoalsOdd2 = firstHalfGoalsOdds
            .FirstOrDefault(o => o.value == "2");

        var mostGoalsHalf1 = mostGoalsHalfOdds
            .FirstOrDefault(o => o.value == "1.");
        
        var mostGoalsHalf2 = mostGoalsHalfOdds
            .FirstOrDefault(o => o.value == "2.");

        var fistHalfDiff = Math.Abs(firstHalfGoalsOdd1.odd.Value - firstHalfGoalsOdd2.odd.Value);
        var mostGoalHalfDiff = Math.Abs(mostGoalsHalf1.odd.Value - mostGoalsHalf2.odd.Value);
        
        var viewModel = new MatchDetailViewModel
        {
            FirstHalfGoals =
            [
                firstHalfGoalsOdd1,
                firstHalfGoalsOddX,
                firstHalfGoalsOdd2
            ],
            MostGoalHalf =
            [
                mostGoalsHalf1,
                mostGoalsHalf2
            ],
            Team1 = model.Team1,
            Team2 = model.Team2,
            GameDate = model.DateTime,
            MatchId = model.MatchID,
            HomeIcon = model.Team1Logo,
            AwayIcon = model.Team2Logo,
            IsValid = mostGoalHalfDiff < 0.75 ? true : false,
            FirstHalfDiff = fistHalfDiff,
            MostGoalHalfDiff = mostGoalHalfDiff
        };


        return View("MatchDetail", viewModel);
    }
}