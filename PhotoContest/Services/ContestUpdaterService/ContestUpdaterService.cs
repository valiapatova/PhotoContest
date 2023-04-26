using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoContest.Data;
using PhotoContest.Models;
using PhotoContest.Models.Enums;

namespace PhotoContest.Services.ContestsUpdaterService
{
    public class ContestsUpdaterService : BackgroundService
    {
        private readonly ILogger<ContestsUpdaterService> logger;
        public ContestsUpdaterService(IServiceProvider services,
            ILogger<ContestsUpdaterService> logger)
        {
            Services = services;
            this.logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private List<ContestUserScore> ChopWinners(List<ContestUserScore> scoreList)
        {
            if (scoreList.Count == 0)
            {
                return new List<ContestUserScore>();
            }

            var topWinners = new List<ContestUserScore>();
            double firstPlaceScore = scoreList.First().Score;

            foreach (var result in scoreList)
            {
                if (result.Score.Equals(firstPlaceScore))
                {
                    topWinners.Add(new ContestUserScore(result.Score, result.user));
                }
            }

            scoreList.RemoveAll(result => result.Score.Equals(firstPlaceScore));

            return topWinners;
        }

        private void UpdateRankPoints(User user, int rankPointsToAdd)
        {
            user.RankPoints += rankPointsToAdd;

            if (user.RankPoints > 1000 && user.Rank != Rank.PhotoDictator)
            {
                user.Rank = Rank.PhotoDictator;
            }
            else if (user.RankPoints > 150 && user.Rank != Rank.Master)
            {
                user.Rank = Rank.Master;
            }
            else if (user.RankPoints > 50 && user.Rank != Rank.Enthusiast)
            {
                user.Rank = Rank.Enthusiast;
            }
        }

        private void Award(List<ContestUserScore> scoreList, int standardRank, int duplicateRank)
        {
            int awardedRankPoints = scoreList.Count >= 2 ? duplicateRank : standardRank;

            foreach (var entry in scoreList)
            {
                UpdateRankPoints(entry.user, awardedRankPoints);
            }
        }

        private void AwardEachParticipant(List<ContestUserScore> scoreList)
        {
            foreach (var entry in scoreList)
            {
                UpdateRankPoints(entry.user, 1);
            }
        }

        private void DistributeRankPoints(List<ContestUserScore> scoreList)
        {
            AwardEachParticipant(scoreList);

            var firstPlaceWinners = ChopWinners(scoreList);
            var secondPlaceWinners = ChopWinners(scoreList);
            var thirdPlaceWinners = ChopWinners(scoreList);

            if (firstPlaceWinners.Count == 0)
            {
                return;
            }

            // First place

            double firstPlaceScore = firstPlaceWinners.First().Score;
            int firstPlaceRankPoints = 50;
            bool doubleDifferenceInTop2 = false;

            if (secondPlaceWinners.Count > 0)
            {
                double secondBestScore = secondPlaceWinners.First().Score;

                if (firstPlaceScore > 2 * secondBestScore || firstPlaceScore.Equals(2 * secondBestScore))
                {
                    firstPlaceRankPoints = 75;
                    doubleDifferenceInTop2 = true;
                }
            }

            if (!doubleDifferenceInTop2 && firstPlaceWinners.Count >= 2)
            {
                firstPlaceRankPoints = 40;
            }

            foreach (var firstPlaceWinner in firstPlaceWinners)
            {
                UpdateRankPoints(firstPlaceWinner.user, firstPlaceRankPoints);
            }

            // Second place
            Award(secondPlaceWinners, 35, 25);

            // Third place
            Award(thirdPlaceWinners, 25, 15);
        }

        /// Returns the sorted list of users by result descending

        private void AwardContestants(Contest contest)
        {
            if (contest.PhaseName != PhaseEnum.Finished)
            {
                return;
            }

            Dictionary<User, double> contestResuts = new Dictionary<User, double>();

            foreach (var user in contest.Users)
            {
                contestResuts[user.User] = 0.0;

                var posts = contest.PhotoPosts.Where(post => post.UserId == user.UserId).ToList();
                double totalScore = 0;

                if (posts.Count > 0)
                {
                    totalScore = posts.First().TotalRating;
                }

                contestResuts[user.User] = totalScore;
            }

            List<ContestUserScore> result = new List<ContestUserScore>();

            foreach (var userResult in contestResuts)
            {
                result.Add(new ContestUserScore(userResult.Value, userResult.Key));
            }

            result.Sort();

            DistributeRankPoints(result);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var contestsService = scope.ServiceProvider.GetRequiredService<ContestsService.IContestsService>();
                    var contests = await contestsService.GetAllContests();

                    foreach (var contest in contests)
                    {
                        if (contest.PhaseName == PhaseEnum.One && DateTime.Now > contest.Phase2Start)
                        {
                            contest.PhaseName = PhaseEnum.Two;
                            context.SaveChanges();
                        }
                        else if (contest.PhaseName == PhaseEnum.Two && DateTime.Now > contest.EndDate)
                        {
                            contest.PhaseName = PhaseEnum.Finished;
                            AwardContestants(contest);
                            context.SaveChanges();
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
