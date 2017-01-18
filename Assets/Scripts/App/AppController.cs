using System;
using UnityEngine;
using Assets.Scripts.Sound;
using Assets.Scripts.Metrics.Model;
using Assets.Scripts.Metrics;
using Assets.Scripts.Settings;
using System.Collections.Generic;

namespace Assets.Scripts.App
{
    public class AppController : MonoBehaviour
    {
        private static AppController appController;
        private AppModel appModel;
		[SerializeField]
		private MetricsController metricsController;
        [SerializeField]
        private List<Game> games;

        void Awake(){
            if (appController == null) appController = this;
            else if (appController != this) Destroy(gameObject);     
            DontDestroyOnLoad(gameObject);
            appModel = new AppModel();
            
        }

		public GameMetrics GetCurrentMetrics()
        {
            return MetricsController.GetController().GetLastMetricOf(appModel.GetCurrentGame());
        }

		public Game GetGameById (int idGame)
		{
			throw new NotImplementedException ();
		}

		public MetricsController GetMetricsController(){
			return metricsController;
		}

        //private void InitModelFromJsonInfo()
        //{

        //    TextAsset JSONstring = Resources.Load("gamesInfo") as TextAsset;
        //    JSONNode data = JSON.Parse(JSONstring.text);
        //    JSONNode calculandox = data["calculandox"];

        //    List<List<List<List<string>>>> gamesInfo = new List<List<List<List<string>>>>(calculandox.Count);
        //    List<List<int>> gamesLevels = new List<List<int>>(calculandox.Count);

        //    for (int area = 0; area < calculandox.Count; area++)
        //    {
        //        gamesInfo.Add(new List<List<List<string>>>(calculandox[area]["games"].Count));
        //        gamesLevels.Add(new List<int>(calculandox[area]["games"].Count));

        //        for (int game = 0; game < calculandox[area]["games"].Count; game++)
        //        {
        //            gamesInfo[area].Add(new List<List<string>>(2)); // name and description
        //            gamesInfo[area][game].Add(new List<string>(2)); // english and spanish name
        //            gamesInfo[area][game].Add(new List<string>(2)); // english and spanish description

        //            for (int languageData = 0; languageData < calculandox[area]["games"][game]["name"].Count; languageData++)
        //            {
        //                gamesInfo[area][game][0].Add(calculandox[area]["games"][game]["name"][languageData].Value);
        //                gamesInfo[area][game][1].Add(calculandox[area]["games"][game]["description"][languageData].Value);
        //            }

        //            gamesLevels[area].Add(calculandox[area]["games"][game]["levels"].AsInt);
        //        }

        //    }


        //    ViewController viewController = ViewController.GetController();
        //    appModel = new AppModel(new List<int> { viewController.GetNumberingGames().Count, viewController.GetGeometryGames().Count, viewController.GetAbilityGames().Count, viewController.GetDataGames().Count }, gamesInfo, gamesLevels);
        //}


        internal string GetGameName(int idGame)
        {
            for (int i = 0; i < games.Count; i++)
            {
				if (games[i].GetId() == idGame) return games[i].GetName();

            }
            return "Error";
        }

      

        internal void PlayCurrentGame()
        {
//            SoundController.GetController().StopMusic();
//            appModel.SetCurrentLevel(level);
//			Debug.Log (level);
//			Debug.Log (GetCurrentLevel());
            // ViewController.GetController().StartGame(appModel.GetCurrentArea(), appModel.GetCurrentGame());
			ViewController.GetController().StartGame(GetCurrentGame());
      
        }

        internal void BackToGame()
        {
            Timer.GetTimer().Resume();
        }

        internal List<Game> GetGames()
        {
            return games;
        }

        internal void SetCurrentArea(int area)
        {
            appModel.SetCurrentArea(area);
        }

        internal void SetCurrentGame(Game currentGame){
            appModel.SetCurrentGame(currentGame);
        }

        public int GetCurrentLevel()
        {
            return appModel.GetCurrentLevel();
        }

        internal Game GetCurrentGame(){
			return appModel.GetCurrentGame ();
        }

        internal void ShowInGameMenu(){
            Timer.GetTimer().Pause();
            ViewController.GetController().ShowInGameMenu();
        }
        /*
        internal string GetActivityName(int area, int game)
        {
            return appModel.GetGameNames()[area][game][SettingsController.GetController().GetLanguage()];
        }
        */

        public static AppController GetController()
        {
            return appController;
        }

        internal int GetCurrentArea()
        {
            return appModel.GetCurrentArea();
        }

		public AppModel GetAppModel(){
			return appModel;
		}

        public void SetCurrentLevel(int level)
        {
            appModel.SetCurrentLevel(level);
        }
    }
}
