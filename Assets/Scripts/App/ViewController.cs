using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.LevelCompleted;
using Assets.Scripts.Settings;
using Assets.Scripts.Sound;
using Assets.Scripts.Games;
using Assets.Scripts.Metrics;
using UnityEngine.UI;

namespace Assets.Scripts.App
{
    public class ViewController : MonoBehaviour
    {
        private static ViewController viewController;

        public GameObject viewPanel;
 
        private GameObject currentGameObject;

        private GameObject inGameMenuScreen;
        private GameObject instructionsScreen;

        void Awake()
        {
            if (viewController == null) viewController = this;
            else if (viewController != this) Destroy(gameObject);      
            DontDestroyOnLoad(this);
        }  

		void Start(){
            //AppController.GetController().SetCurrentGame(3); // Bottle distribution
            //AppController.GetController().SetCurrentLevel(2);
            //StartGame(AppController.GetController().GetCurrentGame());
            //LoadMainMenu();
            LoadCover();
		}

        internal void LoadMainMenu()
        {
            ChangeCurrentObject(LoadPrefab("MainMenu"));
//            if (!SettingsController.GetController().GetMusic()) SoundController.GetController().StopMusic();
//            else SoundController.GetController().PlayMusic();
        }    

        private GameObject LoadPrefab(string name)
        {

            return Resources.Load<GameObject>("Prefabs/" + name);
        }

        internal void LoadCover()
        {
            ChangeCurrentObject(LoadPrefab("Cover"));
        }

     
        internal void LoadMetrics()
        {
           // Destroy(currentGameObject);
           // currentGameObject = Instantiate();
            ChangeCurrentObject(LoadPrefab("Metrics"));
//            SetCanvasScaler(1);

        }

        private void ChangeCurrentObject(GameObject newObject)
        {
//            SetCanvasScaler(0.5f);
            GameObject child = Instantiate(newObject);
            FitObjectTo(child, viewPanel);
            Destroy(currentGameObject);
            currentGameObject = child;            
        }

        internal void ShowInGameMenu()
        {
            if(inGameMenuScreen == null)
            {
                HideInstructions();
                inGameMenuScreen = Instantiate(LoadPrefab("IngameMenu"));
                FitObjectTo(inGameMenuScreen, viewPanel);
            }
                           
        }

        internal void FitObjectTo(GameObject child, GameObject parent)
        {
            child.transform.SetParent(parent.transform, true);
            child.transform.localPosition = Vector3.zero;
            child.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            child.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            child.transform.localScale = Vector3.one;
        }

        internal void RestartCurrentGame()
        {
            currentGameObject.GetComponent<LevelController>().RestartGame();
        }

        internal void LoadSettings()
        {
            ChangeCurrentObject(LoadPrefab("Settings"));
        }

        internal void LoadLogin()
        {
			ChangeCurrentObject(LoadPrefab("NameScreen"));
        }    

        internal void StartGame(int level)
        {
			string game = "";
			switch (level) {
			case 1:
				game = "BedroomActivity";
				break;
			case 2:
				game = "HouseActivity";
				break;
			case 3:
				game = "ClassroomActivity";
				break;
			case 4:
				game = "SchoolActivity";
				break;
			case 5:
				game = "TreasureActivity";
				break;
			case 6:
				game = "NeighbourhoodActivity";
				break;
			case 7:
				game = "PatternsActivity";
				break;
			}

			ChangeCurrentObject(LoadPrefab("Games/" + game));
//            SetCanvasScalerToCurrentGame();
        }

        internal void ShowInstructions()
        {
            instructionsScreen = Instantiate(LoadPrefab("Instructions"));
            FitObjectTo(instructionsScreen, viewPanel);
        }

        internal void HideInGameMenu(){
            Destroy(inGameMenuScreen);
        }

        internal void HideInstructions()
        {
            Destroy(instructionsScreen);
        }

        internal void LoadLevelCompleted()
        {
            ChangeCurrentObject(LoadPrefab("LevelCompleted"));
        }


        public static ViewController GetController()
        {
            return viewController;
        }

        public void SetCanvasScaler(float scale)
        {
			transform.parent.gameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = scale;
        }

        public void SetCanvasScalerToCurrentGame()
        {
              SetCanvasScaler(1);
        }

        public bool IsInGameMenuShowed()
        {
            return inGameMenuScreen != null && inGameMenuScreen.activeSelf;
        }
    }
}
