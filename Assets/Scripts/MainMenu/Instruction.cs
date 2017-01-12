using System;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
    public class Instruction : MonoBehaviour
    {
        public Text[] Texts;
        public bool VisibleAtStart;

        void Start()
        {
            

            /*if (SpanishTexts.Length != Texts.Length) throw new Exception("Diferente cantidad de textos");
            if (SettingsController.GetController().GetLanguage() == 0)
            {
                for (int i = Texts.Length - 1; i >= 0; i--)
                {
                    Texts[i].text = SpanishTexts[i];
                }               
            }
            else
            {
                for (int i = Texts.Length - 1; i >= 0; i--)
                {
                    Texts[i].text = EnglishTexts[i];
                }
            }*/
        }


        void OnEnd()
        {
            GetComponent<Animator>().SetBool("play", false);
            InstructionController.GetController().NextAnimation();
        }

        /*void Start()
    {
        _currentAnimation = 0;
        //animations[_currentAnimation].Play();
        Animation component = Animators[_currentAnimation].GetComponent<Animation>();
        component.Play();
        Invoke("OnEnd", component.clip.length);
    }


    void OnEnd()
    {
        _currentAnimation++;
        if (_currentAnimation < Animators.Length)
        {
            //animations[_currentAnimation].gameObject.SetActive(true);
            Animation component = Animators[_currentAnimation].GetComponent<Animation>();
            component.Play();
            Invoke("OnEnd", component.clip.length);
        }
    }*/
    }
}
