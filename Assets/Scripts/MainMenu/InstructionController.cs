using System;
using System.Collections.Generic;
using Assets.Scripts.App;
using Assets.Scripts.Rules;
using Assets.Scripts.Settings;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
    public class InstructionController : MonoBehaviour
    {
        private static InstructionController _instance;
        private Animation animation;

        public List<Animator> Animators;
        public Button PlayInstructionButton;
        //public Toggle PlayInstructionToggle;
        private static int _currentAnimation;
        private bool _isRunning = false;

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

     

        void OnEnable()
        {
            _isRunning = false;
            PlayInstructionButton.onClick.RemoveAllListeners();
            PlayInstructionButton.onClick.AddListener(InitInstructionAnimation);
            PlayInstructionButton.GetComponent<Animator>().SetBool("stop", true);
            PlayInstructionButton.GetComponent<Animator>().SetBool("play", false);
            Image[] images = PlayInstructionButton.gameObject.GetComponentsInChildren<Image>();
            for (int i = images.Length - 2; i >= 0; i--)
            {
                images[i].enabled = true;

            }
        }

        private void InitInstructionAnimation()
        {
            PlayInstructionButton.onClick.RemoveAllListeners();
            PlayInstructionButton.GetComponent<Animator>().SetBool("stop", false);
            SoundController.GetController().PlayClickSound();
//            Play(AppController.GetController().GetCurrentGame());
            _isRunning = !_isRunning;
            PlayInstructionButton.gameObject.GetComponent<Animator>().SetBool("play", _isRunning);
            Image[] images = PlayInstructionButton.gameObject.GetComponentsInChildren<Image>();
            for (int i = images.Length - 2; i >= 0; i--)
            {
                images[i].enabled = false;
            }          
            PlayInstructionButton.onClick.AddListener(delegate
            {
                _isRunning = !_isRunning;
                PlayInstructionButton.gameObject.GetComponent<Animator>().SetBool("play", _isRunning);
                for (int i = images.Length - 2; i >= 0; i--)
                {
                    images[i].enabled = true;

                }
                foreach (Animator animator in Animators)
                {
                    animator.enabled = _isRunning;
                }
            });
        }

        public void NextAnimation()
        {
            if(_currentAnimation >= 0) Animators[_currentAnimation].SetBool("play", false);

            if (++_currentAnimation < Animators.Count)
            {
                Animators[_currentAnimation].gameObject.transform.SetAsLastSibling();
                Animators[_currentAnimation].gameObject.SetActive(true);
                Animators[_currentAnimation].SetBool("play", true);
            }
            else
            {
                foreach (Animator animator in Animators)
                {
                    animator.gameObject.SetActive(false);
                }
                _isRunning = false;
                PlayInstructionButton.onClick.RemoveAllListeners();
                PlayInstructionButton.onClick.AddListener(InitInstructionAnimation);
                PlayInstructionButton.GetComponent<Animator>().SetBool("stop", true);
                PlayInstructionButton.GetComponent<Animator>().SetBool("play", false);
                Image[] images = PlayInstructionButton.gameObject.GetComponentsInChildren<Image>();
                for (int i = images.Length - 2; i >= 0; i--)
                {
                    images[i].enabled = true;

                }
            }

        }

        public static InstructionController GetController()
        {
            return _instance;
        }

        public void Play(Game game)
        {
//            string[] textStrings = SettingsController.GetController().GetLanguage() == 0
//                    ? game.SpanishInstructionStrings
//                    : game.EnglishInstructionStrings;
//            List<Text> texts = new List<Text>();
//            for (int i = 0; i < Animators.Count; i++)
//            {
//                Instruction instruction = Animators[i].gameObject.GetComponent<Instruction>();
//                texts.AddRange(instruction.Texts);
//                instruction.gameObject.SetActive(instruction.VisibleAtStart);
//                Animators[i].SetBool("play", false);
//            }
//
//            if(texts.Count != textStrings.Length) throw new Exception("Diferente cantidad");
//             
//            for (int i = textStrings.Length - 1; i >= 0; i--)
//            {
//                texts[i].text = textStrings[i];
//            }
//
//
//
//
//            _currentAnimation = -1;
//            NextAnimation();
        }
    }
}