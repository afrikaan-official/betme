using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using BttMe.Data;
using Microsoft.AspNetCore.Mvc;
using BttMe.Models;
using Newtonsoft.Json.Linq;

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

    // public async Task<IActionResult> Index()
    // {
    //     var response = await _httpClient.GetAsync($"bettable-matches?type=1&date=2025-04-27");
    //     var matchData = await response.Content.ReadAsStringAsync();
    //
    //     //var matchData = await System.IO.File.ReadAllTextAsync(Environment.CurrentDirectory + "/Data/matches.json");
    //     var model = JsonSerializer.Deserialize<MatchResponse>(matchData);
    //
    //     var matchDetailTasks = model.data.Select(x => FetchMatchDetail(x.MatchID)).ToList();
    //     var result = await Task.WhenAll(matchDetailTasks);
    //     var vm = new MatchListViewModel();
    //
    //     foreach (var match in model.data)
    //     {
    //         vm.MatchVMList.Add(new MatchViewModel
    //         {
    //             MatchID = match.MatchID,
    //             DateTime = match.DateTime,
    //             LeagueFlag = match.LeagueFlag,
    //             Team1 = match.Team1,
    //             Team2 = match.Team2,
    //             HomeWin = match.HomeWin,
    //             Draw = match.Draw,
    //             AwayWin = match.AwayWin,
    //             MostGoalsHalf1 = result.First(x => x.Item1 == match.MatchID).Item2,
    //             MostGoalsHalf2 = result.First(x => x.Item1 == match.MatchID).Item3,
    //             FirstHalf1 = result.First(x => x.Item1 == match.MatchID).Item4,
    //             FirstHalf2 = result.First(x => x.Item1 == match.MatchID).Item5,
    //         });
    //     }
    //
    //
    //     return View(vm);
    // }

    public async Task<IActionResult> Index()
    {
        var vm = new MatchListViewModel();
        var response =
            await _httpClient.GetAsync(
                "https://www.bilyoner.com/api/v3/mobile/aggregator/gamelist/all/v1?tabType=1&bulletinType=2");
        var matchData = await response.Content.ReadAsStringAsync();
        //var matchData = await System.IO.File.ReadAllTextAsync(Environment.CurrentDirectory + "/Data/bilyoner_home.json");
        var model = JsonSerializer.Deserialize<BilyonerHomeData>(matchData,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        vm.MatchVMList.AddRange(model.Events.Values.Select(x => new MatchViewModel
        {
            MatchID = x.Id,
            DateTime = x.Esd,
            Date = x.Esd.ToString("yyyy-MM-dd HH:mm:ss"),
            MatchName = x.N
        }).ToList().OrderBy(x => x.DateTime));

        // var matchMarkets= await System.IO.File.ReadAllTextAsync(Environment.CurrentDirectory + "/Data/match-markets.json");
        // var matchMarketsData = JsonSerializer.Deserialize<BilyonerHomeData>(matchMarkets, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var eventTasks = model.Events.Keys.Select(x => FetchEvents(int.Parse(x)));
        var eventData = await Task.WhenAll(eventTasks);

        foreach (var e in eventData)
        {
            if (e.Item2 != 0)
            {
                vm.AddOdds(e.Item1, e.Item2, e.Item3, e.Item4, e.Item5);
            }
            else
            {
                vm.MatchVMList.Remove(vm.MatchVMList.First(x => x.MatchID == e.Item1));
            }
        }

        return View(vm);
    }

    // [HttpGet]
    // public async Task<ViewResult> MatchDetail(int gameId)
    // {
    //     var response = await _httpClient.GetAsync($"bettable-matches/details?matchID={gameId}");
    //
    //     if ((int)response.StatusCode == 401)
    //     {
    //         return View("401");
    //     }
    //
    //     var matchDetailData = await response.Content.ReadAsStringAsync();
    //     //var matchDetailData = await System.IO.File.ReadAllTextAsync(Environment.CurrentDirectory + "/Data/detail.json");
    //     var md = JsonSerializer.Deserialize<MatchDetail>(matchDetailData);
    //     var model = md.data[0];
    //
    //     if (!model.Bets.Any(x => x.gameName == "Hangi Yarıda Daha Fazla Gol Olur") ||
    //         !model.Bets.Any(x => x.gameName == "İlk Yarı Sonucu"))
    //     {
    //         return View("MatchDetail", new MatchDetailViewModel
    //         {
    //             FirstHalfGoals = [],
    //             MostGoalHalf = [],
    //             Team1 = model.Team1,
    //             Team2 = model.Team2,
    //             GameDate = model.DateTime,
    //             MatchId = model.MatchID,
    //             HomeIcon = model.Team1Logo,
    //             AwayIcon = model.Team2Logo,
    //             IsValid = false,
    //             FirstHalfDiff = 0,
    //             MostGoalHalfDiff = 0
    //         });
    //     }
    //
    //     var firstHalfGoalsOdds = model.Bets
    //         .Where(b => b.gameName == "İlk Yarı Sonucu")
    //         .SelectMany(b => b.odds);
    //
    //     var mostGoalsHalfOdds = model.Bets
    //         .Where(b => b.gameName == "Hangi Yarıda Daha Fazla Gol Olur")
    //         .SelectMany(b => b.odds);
    //
    //     var firstHalfGoalsOdd1 = firstHalfGoalsOdds
    //         .FirstOrDefault(o => o.value == "1");
    //
    //     var firstHalfGoalsOddX = firstHalfGoalsOdds
    //         .FirstOrDefault(o => o.value == "0");
    //
    //     var firstHalfGoalsOdd2 = firstHalfGoalsOdds
    //         .FirstOrDefault(o => o.value == "2");
    //
    //     var mostGoalsHalf1 = mostGoalsHalfOdds
    //         .FirstOrDefault(o => o.value == "1.");
    //
    //     var mostGoalsHalf2 = mostGoalsHalfOdds
    //         .FirstOrDefault(o => o.value == "2.");
    //
    //     var fistHalfDiff = Math.Abs(firstHalfGoalsOdd1.odd.Value - firstHalfGoalsOdd2.odd.Value);
    //     var mostGoalHalfDiff = Math.Abs(mostGoalsHalf1.odd.Value - mostGoalsHalf2.odd.Value);
    //
    //     var viewModel = new MatchDetailViewModel
    //     {
    //         FirstHalfGoals =
    //         [
    //             firstHalfGoalsOdd1,
    //             firstHalfGoalsOddX,
    //             firstHalfGoalsOdd2
    //         ],
    //         MostGoalHalf =
    //         [
    //             mostGoalsHalf1,
    //             mostGoalsHalf2
    //         ],
    //         Team1 = model.Team1,
    //         Team2 = model.Team2,
    //         GameDate = model.DateTime,
    //         MatchId = model.MatchID,
    //         HomeIcon = model.Team1Logo,
    //         AwayIcon = model.Team2Logo,
    //         IsValid = mostGoalHalfDiff < 0.75 ? true : false,
    //         FirstHalfDiff = fistHalfDiff,
    //         MostGoalHalfDiff = mostGoalHalfDiff
    //     };
    //
    //
    //     return View("MatchDetail", viewModel);
    // }


    // private async Task<(int, double, double, double, double)> FetchMatchDetail(int matchId)
    // {
    //     var fistHalfDict = new Dictionary<string, List<double>>();
    //     var response = await _httpClient.GetAsync($"bettable-matches/details?matchID={matchId}");
    //
    //     var matchDetailData = await response.Content.ReadAsStringAsync();
    //     //var matchDetailData = await System.IO.File.ReadAllTextAsync(Environment.CurrentDirectory + "/Data/detail.json");
    //     var md = JsonSerializer.Deserialize<MatchDetail>(matchDetailData);
    //     var model = md.data[0];
    //
    //     if (!model.Bets.Any(x => x.gameName == "Hangi Yarıda Daha Fazla Gol Olur") ||
    //         !model.Bets.Any(x => x.gameName == "İlk Yarı Sonucu"))
    //     {
    //         //veri yok
    //         //devam
    //     }
    //
    //     var firstHalfGoalsOdds = model.Bets
    //         .Where(b => b.gameName == "İlk Yarı Sonucu")
    //         .SelectMany(b => b.odds);
    //
    //     var mostGoalsHalfOdds = model.Bets
    //         .Where(b => b.gameName == "Hangi Yarıda Daha Fazla Gol Olur")
    //         .SelectMany(b => b.odds);
    //
    //     var firstHalfGoalsOdd1 = firstHalfGoalsOdds
    //         .FirstOrDefault(o => o.value == "1");
    //
    //     var firstHalfGoalsOddX = firstHalfGoalsOdds
    //         .FirstOrDefault(o => o.value == "0");
    //
    //     var firstHalfGoalsOdd2 = firstHalfGoalsOdds
    //         .FirstOrDefault(o => o.value == "2");
    //
    //     var mostGoalsHalf1 = mostGoalsHalfOdds
    //         .FirstOrDefault(o => o.value == "1.");
    //
    //     var mostGoalsHalf2 = mostGoalsHalfOdds
    //         .FirstOrDefault(o => o.value == "2.");
    //
    //
    //     if (firstHalfGoalsOdd1 != null && mostGoalsHalf1 != null)
    //     {
    //
    //         return (matchId, mostGoalsHalf1.odd.Value, mostGoalsHalf2.odd.Value, firstHalfGoalsOdd1.odd.Value, firstHalfGoalsOdd2.odd.Value);
    //     }
    //
    //
    //     return (matchId, 0, 0, 0, 0);
    // }

    public async Task<(int, double, double, double, double)> FetchEvents(int matchId)
    {
        try
        {
            var allEventsResponse =
                await _httpClient.GetAsync(
                    $"https://www.bilyoner.com/api/v3/mobile/aggregator/match-card/{matchId}/odds?isLiveEvent=false&isPopular=false");

            var allEventData = await allEventsResponse.Content.ReadAsStringAsync();
            var root = JObject.Parse(allEventData);

            var filteredMostGoals =
                root.SelectTokens(
                        "$.oddGroupTabs[?(@.id == 1)].matchCardOdds[?(@.name == 'Hangi Yarıda Daha Fazla Gol Atılır?')].oddList[:].val")
                    .ToList();
            var fisrtHalfGoals =
                root.SelectTokens(
                        "$.oddGroupTabs[?(@.id == 1)].matchCardOdds[?(@.name == 'İlk Yarı Sonucu')].oddList[:].val")
                    .ToList();

            if (filteredMostGoals.Any() && fisrtHalfGoals.Any())
            {
                return (matchId, filteredMostGoals[0].Value<double>(), filteredMostGoals[2].Value<double>(),
                    fisrtHalfGoals[0].Value<double>(), fisrtHalfGoals[2].Value<double>());
            }

            return (matchId, 0, 0, 0, 0);
        }
        catch (Exception e)
        {
            return (matchId, 0, 0, 0, 0);
        }
    }
}