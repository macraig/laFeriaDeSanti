using Assets.Scripts.App;
using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{

    public class MainMenuController : MonoBehaviour
    {
        private static MainMenuController mainMenuController;

        public MenuView menuView;
        public GamePreview gamePreview;

        [SerializeField]
        // numbering, geometry, ability, data
       
        void Awake()
        {
            if (mainMenuController == null) mainMenuController = this;
            else if (mainMenuController != this) Destroy(gameObject);
        }

        void Start()
        {
        
        }

        public static MainMenuController GetController()
        {
            return mainMenuController;
        }

        internal void ShowMenu()
        {
            menuView.gameObject.SetActive(true);
        }

        internal void PlayCurrentGame(int level)
        {
            AppController.GetController().PlayCurrentGame(level);
        }
      
		internal void GoBack() {
			ViewController.GetController().LoadLogin();
		}

        internal void ShowPreviewGame(int id)
        {
            //AppController.GetController().SetCurrentGame(0); // todo
            // AppController.GetController().SetCurrentArea(game.GetArea());
            ViewController.GetController().SetCanvasScaler(1f);
            AppController.GetController().SetCurrentGame(id);
//            gamePreview.SetGameDescription(game.GetDescriptions()[SettingsController.GetController().GetLanguage()]);
//            gamePreview.SetPossibleLevels(game.GetLevels());
           // gamePreview.SetImages(game.GetIcon(), areaColors[game.GetArea()]);

            gamePreview.gameObject.SetActive(true);
//            menuView.gameObject.SetActive(false);

//            foreach (Animator animator in InstructionController.GetController().Animators)
//            {
//                Destroy(animator.gameObject);
//            }
           


           
        }

        internal void ShowSettings()
        {
            ViewController.GetController().LoadSettings();
        }

        internal void ShowMetrics()
        {
            ViewController.GetController().LoadMetrics();
        }

      

        internal List<Game> Search(List<int> areas, string text)
        {

            List<Game> gamesToShow = new List<Game>();
            string searchText = text.ToLower();
            foreach (Game game in AppController.GetController().GetGames())
            {
                string gameName = game.GetNames()[SettingsController.GetController().GetLanguage()].ToLower();
                string gameNameSimplified = gameName.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').ToLower();
                if (areas.Contains(game.GetArea()) && (gameName.Contains(searchText) || gameNameSimplified.Contains(searchText)))
                {
                    gamesToShow.Add(game);
                }
            }
           

            return gamesToShow;
            
        }

       
    }
}