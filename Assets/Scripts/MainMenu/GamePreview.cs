using System;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;
using System.Collections.Generic;
using Assets.Scripts.App;

namespace Assets.Scripts.MainMenu
{

    public class GamePreview : MonoBehaviour
    {

        public Text levelLabel;
 	    public Text gameDescription;
        [SerializeField]
        private Image levelImage;
        [SerializeField]
        private Button playButton;
   

        private int leveLSelected;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickBackBtn();
            }
        }

     
        internal void SetGameDescription(string description)
        {
            this.gameDescription.text = description.Replace("/n", "\n");
        }

     
        internal void SetImages(Sprite areaSprite)
        {
            levelImage.sprite = areaSprite;      
        }

        public void OnClickPlayBtn()
        {
            ClickSound();
           /* int levelSelected = 0;
            for (; levelSelected < levelToggles.Count && levelToggles[levelSelected].isOn; levelSelected++)*/
			Debug.Log("level selected"+leveLSelected);
			MainMenuController.GetController().PlayCurrentGame(AppController.GetController().GetCurrentLevel());
        }

        public void OnClickBackBtn()
        {
            ViewController.GetController().SetCanvasScaler(0.5f);
            ClickSound();
            MainMenuController.GetController().ShowMenu();
            gameObject.SetActive(false);
        }

        public void OnClickLvlBtn(int level)
        {
            leveLSelected = level;     
            ClickSound();
        }

        internal void ClickSound()
        {
            SoundController.GetController().PlayClickSound();
        }

        public void SetInstruction(GameObject[] instructionsSequence)
        {
            throw new NotImplementedException();
        }
    }
}
