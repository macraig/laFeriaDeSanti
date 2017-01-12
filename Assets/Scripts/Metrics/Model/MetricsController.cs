using UnityEngine;
using Assets.Scripts.Settings;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using Assets.Scripts.App;
using Assets.Scripts.Metrics.Model;
using Assets.Scripts.Games;

namespace Assets.Scripts.Metrics
{
    public class MetricsController : MonoBehaviour
    {
        private static MetricsController metricsController;
        public MetricsModel metricsModel;
        // this list is to register the answers of the currentSublevel. It's is restarted
        // when a sublevel change 
        private List<List<bool>> actualBuffer;

        void Awake()
        {
            if (metricsController == null) metricsController = this;
            else if (metricsController != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            metricsModel = new MetricsModel(AppController.GetController().GetGames());
            // list to first exercise is added
            actualBuffer = new List<List<bool>> {new List<bool>()};
        }

        internal GameMetrics GetLastMetricOf(int game, int level)
        {
            List<List<GameMetrics>> metrics = metricsModel.SearchMetricsByGame(game);
            return metrics[level][metrics[level].Count - 1];
        }

        internal List<MetricsGroup> GetMetrics()
        {
            return metricsModel.GetMetrics();
        }

        internal GameMetrics GetCurrentMetrics()
        {
            return metricsModel.GetCurrentMetrics();
        }

        internal List<GameMetrics> GetMetricsByLevel(int game, int level)
        {
            return metricsModel.SearchMetricsByGame(game)[level];
        }
        
        public void GameStart()
        {
            metricsModel.GameStarted();
            RestartActualBuffer();
            Timer.GetTimer().InitTimer();
        }

        private void RestartActualBuffer()
        {
            actualBuffer.Clear();
            actualBuffer.Add(new List<bool>());
        }

        public void GameFinished(int minSeconds, int pointsPerSecond, int pointsPerError)
        {
            Timer.GetTimer().FinishTimer();
            metricsModel.GameFinished(Timer.GetTimer().GetLapsedSeconds(), minSeconds, pointsPerSecond, pointsPerError);
            saveToDisk();
        }

        public void GameFinished(MetricsTable metricsTable)
        {
            Timer.GetTimer().FinishTimer();
            //metricsModel.GameFinished(Timer.GetTimer().GetLapsedSeconds(), minSeconds, pointsPerSecond, pointsPerError);
            metricsModel.GameFinished(Timer.GetTimer().GetLapsedSeconds(), metricsTable);
            saveToDisk();
        }

        internal GameMetrics GetBestMetric(int area, int game, int level)
        {
            return metricsModel.GetBestMetric(game, level);
        }

        internal float GetMaxScore()
        {
            return metricsModel.GetMaxScore();
        }

        private void saveToDisk()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + SettingsController.GetController().GetUsername() + ".dat");
            bf.Serialize(file, metricsModel);
            file.Close();
        }

        public void LoadFromDisk()
        {
            if (File.Exists(Application.persistentDataPath + "/" + SettingsController.GetController().GetUsername() + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + SettingsController.GetController().GetUsername() + ".dat", FileMode.Open);
                metricsModel = (MetricsModel)bf.Deserialize(file);
                file.Close();

                metricsModel.UpdateGames(AppController.GetController().GetGames());
            } else
            {
                metricsModel = new MetricsModel(AppController.GetController().GetGames());
            }
        }

        internal static MetricsController GetController()
        {
            return metricsController;
        }

        public void AddRightAnswer()
        {
            GetCurrentMetrics().AddRightAnswer();
            actualBuffer[actualBuffer.Count - 1].Add(true);

            // the first answer is the most important
            if (actualBuffer[actualBuffer.Count - 1].Count == 1)
            {
                GetCurrentMetrics().CheckIfPassedToNextSubLevel(actualBuffer);
            }           
            // always the next list is initialized
            actualBuffer.Add(new List<bool>());
        }

        public void AddWrongAnswer()
        {
            GetCurrentMetrics().AddWrongAnswer();
            actualBuffer[actualBuffer.Count - 1].Add(false);

            // the first answer is the most important
            if (actualBuffer[actualBuffer.Count - 1].Count == 1)
            {
                GetCurrentMetrics().CheckIfDownSubLevel(actualBuffer);
            }

  

        }

//        private bool CheckSurrenderPossibility()
//        {
//            return actualBuffer[actualBuffer.Count - 1].Count == 2;
//
//        }

        public int GetCurrentSubLevel()
        {
            return GetCurrentMetrics().GetCurrentSubLevel();
        }

        public void OnSurrender()
        {
            actualBuffer.Add(new List<bool>());
        }
    }
}