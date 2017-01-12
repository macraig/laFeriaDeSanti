using System.Collections.Generic;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;
using Assets.Scripts.App;
using Assets.Scripts.MainMenu;

namespace Assets.Scripts.Rules
{
    public class GameRules : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] areaSprites;
        private List<Color> areaColors;

        [SerializeField]
        private Text rulesLabel;
        [SerializeField]
        private Text backToGameLabel;
        [SerializeField]
        private Text gameDescription;
        public Text InstructionsTitleText;
        [SerializeField]
        private List<Image> areaColorImages;
        [SerializeField]
        private Image icon;

        public Button PlayInstructionButton;
        public GameObject InstructionPanel;




        void Start(){
            ViewController.GetController().SetCanvasScaler(1f);
            areaColors = new List<Color>{ new Color32(237, 164, 60, 255), new Color32(209, 83, 51, 255), new Color32(26, 171, 209, 255), new Color32(11, 34, 86, 255) };
           
//            Game currentGame = AppController.GetController().GetCurrentGame();
//            gameDescription.text = currentGame.GetDescriptions()[SettingsController.GetController().GetLanguage()];
//            SetArea(currentGame.GetArea());
//            icon.sprite = currentGame.GetIcon();



            foreach (Animator animator in InstructionController.GetController().Animators)
            {
                Destroy(animator.gameObject);
            }
            InstructionController.GetController().Animators.Clear();
//            foreach (GameObject instruction in currentGame.InstructionsSequence)
//            {
//                GameObject instantiate = Instantiate(instruction);
//                ViewController.GetController().FitObjectTo(instantiate, InstructionPanel);
//                InstructionController.GetController().Animators.Add(instantiate.GetComponent<Animator>());
//            }

            foreach (Instruction instruction in InstructionPanel.GetComponentsInChildren<Instruction>(true))
            {
                instruction.gameObject.SetActive(false);
            }

            //Animator[] componentsInChildren = gamePreview.InstructionPanel.GetComponentsInChildren<Animator>(true);
            /* for (int i = 0; i < componentsInChildren.Length ; i++)
             {
                 InstructionController.GetController().Animators.Add(componentsInChildren[i]);
             }*/
          
        }

      

        internal void SetGameDescription(string description)
        {
            this.gameDescription.text = description;
        }

        internal void SetArea(int area)
        {
            UpdateAreaImages(areaColors[area]);
        }

        private void UpdateAreaImages(Color color)
        {
            for (int i = 0; i < areaColorImages.Count; i++)
            {
                areaColorImages[i].color = color;
            }
        }

        public void OnClickBackToGame()
        {
            ViewController.GetController().SetCanvasScalerToCurrentGame();
            ClickSound();
            ViewController.GetController().HideInstructions();
        }

        private void ClickSound()
        {
            SoundController.GetController().PlayClickSound();
        }
    }
}