using System;
using System.Collections.Generic;
using Assets.Scripts.App;
using Assets.Scripts.Metrics.Model;
using UnityEngine;

namespace Assets.Scripts.Metrics{
    [Serializable]
	public class MetricsModel 
	{
        private const int MAX_SCORE = 10000;
        private const int MIN_SCORE = 500;
        
        private GameMetrics currentMetric;

		public List<MetricsGroup> metrics;
        
		public MetricsModel(List<Game> games){
           CreateLevelMetrics(games);
		}
        internal GameMetrics GetCurrentMetrics()
        {
            return currentMetric;
        }

        internal void GameStarted()
        {
            AppController appController = AppController.GetController();
//            Game currentGame = appController.GetCurrentGame();
//            currentMetric = new GameMetrics(appController.GetCurrentArea(), currentGame.GetId(), appController.GetCurrentLevel(), currentGame.GetExercisesBySubLevel());
        }
        
        private void CreateLevelMetrics(List<Game> games)
        {
            metrics = new List<MetricsGroup>(games.Count);

            for (int i = 0; i < games.Count; i++)
            {
                metrics.Add(new MetricsGroup(games[i].GetId()));
            }
      
        }    

        internal void GameFinished(int lapsedSeconds, int minSeconds, int pointsPerSecond, int pointsPerError)
        {
            currentMetric.SetLapsedSeconds(lapsedSeconds);
            CalculateFinalScore(currentMetric, minSeconds, pointsPerSecond, pointsPerError);
            CalculeteStars(currentMetric);
            SaveMetric(currentMetric);
        }

        internal void GameFinished(int lapsedSeconds, MetricsTable metricsTable)
        {
            currentMetric.SetLapsedSeconds(lapsedSeconds);
            CalculateFinalScore(currentMetric, metricsTable);
            CalculeteStars(currentMetric);
            SaveMetric(currentMetric);
        }

        private void SaveMetric(GameMetrics currentMetric)
        {
            for (int i = 0; i < metrics.Count; i++)
            {
                if(metrics[i].GetGameId() == currentMetric.GetIndex())
                {
                    metrics[i].GetMetrics()[currentMetric.GetLevel()].Add(currentMetric);
                    break;
                }
            }
        }


        internal List<MetricsGroup> GetMetrics()
        {
            return metrics;
        }

        internal void UpdateGames(List<Game> currentGames)
        {
            bool gamesExists;
            for (int i = 0; i < currentGames.Count; i++)
            {
                gamesExists = false;
                for (int j = 0; j < metrics.Count; j++)
                {
                    if (metrics[j].GetGameId() == currentGames[i].GetId()) gamesExists = true; break;
                }

                if(!gamesExists) metrics.Add(new MetricsGroup(currentGames[i].GetId()));
            }
        }


        internal float GetMaxScore()
        {
            return MAX_SCORE + 0f;
        }
  
        internal GameMetrics GetBestMetric(int game, int level)
        {
            List<List<GameMetrics>> gameMetrics = SearchMetricsByGame(game);
            if (gameMetrics == null || gameMetrics[level].Count == 0) return null;

            GameMetrics max = gameMetrics[level][0];
            for (int i = 1; i < gameMetrics[level].Count; i++)
            {
                if (gameMetrics[level][i].GetScore() > max.GetScore()) { max = gameMetrics[level][i]; }
            }
            return max;                
        }

        public List<List<GameMetrics>> SearchMetricsByGame(int game)
        {
            for (int i = 0; i < metrics.Count; i++)
            {
                if (metrics[i].GetGameId() == game) return metrics[i].GetMetrics();
            }
            return null;
        }


        private void CalculateFinalScore(GameMetrics gameMetrics, int minSeconds, int pointsPerSecond, int pointsPerError)
        {
            gameMetrics.SetScore(MAX_SCORE - pointsPerSecond * (gameMetrics.GetLapsedSeconds() < minSeconds ? 0 : gameMetrics.GetLapsedSeconds() - minSeconds) - gameMetrics.GetWrongAnswers() * pointsPerError);
            if (gameMetrics.GetScore() < MIN_SCORE) gameMetrics.SetScore(MIN_SCORE);

        }

        private void CalculateFinalScore(GameMetrics gameMetrics, MetricsTable metricsTable)
        {
            for (int i = 0; i < metricsTable.RangeMetricsInfos.Count; i++)
            {
                RangeMetricsInfo currentRangeMetricInfo = metricsTable.RangeMetricsInfos[i];
                if (gameMetrics.GetWrongAnswers() <= currentRangeMetricInfo.MaxErrors &&
                    gameMetrics.GetLapsedSeconds() <= currentRangeMetricInfo.MaxTime)
                {
                    Range range = (Range) (i);
                    float deltaRange = (range.GetDelta() / (currentRangeMetricInfo.MaxErrors - currentRangeMetricInfo.MinErrors + 1));
                    float baseForErrorsQuantity = deltaRange * ( currentRangeMetricInfo.MaxErrors - gameMetrics.GetWrongAnswers());
                    float score = range.GetMinScore() + baseForErrorsQuantity;
                    float bonusFromTime = 0;
                    if (gameMetrics.GetLapsedSeconds() <= currentRangeMetricInfo.MaxTimeToBonus)
                    {
                        float secondsToSubtract = gameMetrics.GetLapsedSeconds() - metricsTable.FreeTime;
                        bonusFromTime = deltaRange;
                        if (secondsToSubtract > 0)
                        {
                            bonusFromTime -= secondsToSubtract*(deltaRange / currentRangeMetricInfo.MaxTimeToBonus);
                        }
                    }
                    score += bonusFromTime;

                    gameMetrics.SetScore(Mathf.RoundToInt(score));
                    gameMetrics.SetBonusTime(Mathf.RoundToInt(bonusFromTime));
                    gameMetrics.SetRange(range);
                    return;
                }
            }

            gameMetrics.SetScore(500);
            gameMetrics.SetBonusTime(0);
            gameMetrics.SetRange(Range.Rookie);
        }

        private void CalculeteStars(GameMetrics gameMetrics)
        {
            float percentage = (gameMetrics.GetScore() + 0f) / (MAX_SCORE + 0f);
            gameMetrics.SetStars(percentage > 0.85 ? 3 : percentage > 0.5 ? 2 : 1);
        }
    }
}

