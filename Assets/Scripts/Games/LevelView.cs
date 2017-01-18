using Assets.Scripts.App;
using Assets.Scripts.Metrics;
using Assets.Scripts.Metrics.Model;
using Assets.Scripts.Sound;
using Assets.Scripts.Common;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.Games
{
    // His childs have to implement a method called NextChallenge that
    // recieve whatever it needs
    public abstract class LevelView : MonoBehaviour
    {
//      
		//Ingame Menu Panel
		public GameObject menuPanel;
		//End Game Panel
		public GameObject endGamePanel;
		//Explanation Panel
		public GameObject explanationPanel;
		public AudioClip explanationSound;

		public GameObject starPanel;
		private Sprite star;




		/*-----Functions for menuPanel panel-----*/

		public void OnClickMenuBtn(){
			PlaySoundClick();
			ShowInGameMenu();
		}

		public void ShowInGameMenu(){
			menuPanel.SetActive (true);
		}

		public void HideInGameMenu(){
			PlaySoundClick ();
			menuPanel.SetActive (false);
		}

		public void OnClickInstructionsButton(){
			PlaySoundClick ();
			HideInGameMenu ();
			ShowExplanation ();

		}

		public void OnHoverInstructionsButton(){
			menuPanel.GetComponentInChildren<Text> ().text = "CONSIGNA";
		}

		public void OnHoverRestartButton(){
			menuPanel.GetComponentInChildren<Text> ().text = "VOLVER A JUGAR";

		}

		public void OnHoverQuitButton(){
			menuPanel.GetComponentInChildren<Text> ().text = "VOLVER AL MENÚ";

		}

		public void OnExitHover(){
			menuPanel.GetComponentInChildren<Text> ().text = "";
		}

		public void OnClickRestartButton(){
			PlaySoundClick ();
//			HideInGameMenu ();
			RestartGame ();

		}

		public void OnClickExitGameButton(){
			PlaySoundClick ();
//			HideInGameMenu ();
			ExitGame ();

		}

		/*-----Functions for finalResult panel-----*/
	


		public void ShowEndPanel(){
			endGamePanel.SetActive (true);
			SoundController.GetController ().PlayLevelCompleteSound ();
			ShowStars ();
		}

		void ShowStars ()
		{
			star = Resources.Load<Sprite> ("Sprites/star");
			int stars = AppController.GetController ().GetCurrentMetrics ().GetStars ();
			for(int i = 0; i < stars; i++)
			{            
				Image starImage = starPanel.GetComponentsInChildren<Image> (true) [i+1];
				starImage.sprite=star;
			}
		}			

		public void OnEndHoverRestartButton(){
			endGamePanel.GetComponentInChildren<Text> ().text = "VOLVER A JUGAR";

		}

		public void OnEndHoverQuitButton(){
			endGamePanel.GetComponentInChildren<Text> ().text = "VOLVER AL MENÚ";

		}

		public void OnEndExitHover(){
			endGamePanel.GetComponentInChildren<Text> ().text = "";
		}

		/*------------------------------------------*/



		internal void ShowExplanation(){
			explanationPanel.SetActive(true);
			SoundController.GetController ().PlayClip (explanationSound);
		}


		public void HideExplanation(){
			PlaySoundClick ();
			explanationPanel.SetActive (false);

		}





		internal void ExitGame(){
//			MetricsController.GetController().DiscardCurrentMetrics();
			ViewController.GetController().LoadMainMenu();
		}

        // This method have to restart the view of the game to the initial state
		public  void RestartGame(){
			ViewController.GetController ().RestartCurrentGame ();
		}

        // This method have to be called when the user clicks menuButton
       
        public void OnClickSurrender()
        {
            PlaySoundClick();
//            LevelController.GetLevelController().ResolveExercise();

            MetricsController.GetController().OnSurrender();
        }
        // This method have to be called when the user clicks a button
        internal void PlaySoundClick()
        {
            SoundController.GetController().PlayClickSound();
        }
        // This method have to be called when the answers is correct
        internal void PlayRightSound()
        {
            SoundController.GetController().PlayRightAnswerSound();
        }
        // This method have to be called when the answers is incorrect
        internal void PlayWrongSound()
        {
            SoundController.GetController().PlayFailureSound();
        }
			

        public void OnClickNextButton()
        {
            PlaySoundClick();
            LevelController.GetLevelController().NextChallenge();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { OnClickMenuBtn(); }
        }


		public void EndGame(int minSeconds, int pointsPerSecond, int pointsPerError){
			MetricsController.GetController().GameFinished(minSeconds, pointsPerSecond, pointsPerError);
			ShowEndPanel ();
		}

    }
}
