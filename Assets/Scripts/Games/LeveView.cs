using Assets.Scripts.App;
using Assets.Scripts.Metrics;
using Assets.Scripts.Sound;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Games
{
    // His childs have to implement a method called NextChallenge that
    // recieve whatever it needs
    public abstract class LevelView : MonoBehaviour
    {
//      

        // This method have to restart the view of the game to the initial state
        public abstract void RestartGame();

        // This method have to be called when the user clicks menuButton
        public void OnClickMenuBtn(){
            PlaySoundClick();
            AppController.GetController().ShowInGameMenu();
        }

        public void OnClickSurrender()
        {
            PlaySoundClick();
            LevelController.GetLevelController().ResolveExercise();

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
    }
}
