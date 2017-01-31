﻿using Assets.Scripts.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Picadura de la Cobra Gay
namespace Assets.Scripts.Games
{
    public class RaulSaysController : MonoBehaviour
    {

        public enum Stages { Arrow, Sound, Word}

        private List<Stages> allStages;

        public static RaulSaysController instance;

        [SerializeField]
        Sprite[] arrows;

        [SerializeField]
        AudioClip[] audios;


        [SerializeField]
        Sprite[] animalSprites;

        RaulLevel currentLevel;
        
        public RaulSaysView view;

        private int correctAnswers;
        private int incorrectAnswers;

        private int currentStage;
        private int currentLevelCounter;

        private Dictionary<Stages,RaulStage> stageDictionary;

        private int currentTime;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }else
            {
                Destroy(gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
            incorrectAnswers = 0;

            view = GetComponent<RaulSaysView>();

            allStages = new List<Stages>();
            allStages.Add(Stages.Arrow);
            allStages.Add(Stages.Sound);
            allStages.Add(Stages.Word);

            stageDictionary = new Dictionary<Stages, RaulStage>();
            stageDictionary.Add(Stages.Arrow, new RaulArrowStage(arrows));
            stageDictionary.Add(Stages.Sound, new RaulSoundStage(audios));
            stageDictionary.Add(Stages.Word, new RaulWordStage());
            currentLevelCounter = -1;
            ChangeToNextLevel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CorrectOptionChosen()
        {
            SoundController.GetController().PlayRightAnswerSound();
            correctAnswers++;
            view.ShowCorrectAnimation();

            if(correctAnswers == 3 && currentLevelCounter != 2)
            {
                RaulStage nextStage = GetNextStage();
                if(nextStage == null)
                {
                    ChangeToNextLevel();
                }else
                {
                    StartCoroutine(GetNextOption(nextStage, 2));
                }
            }
            else if(currentLevelCounter == 2)
            {
                if (correctAnswers < 10 && correctAnswers % 3 == 0)
                {
                    currentTime += 7;
                }
                else if (correctAnswers % 3 == 0)
                {
                    currentTime += 5;
                }
                CancelInvoke("RunTime");
                view.SetTime(currentTime);
                RandomizeStage(2);
            }
            else
            {
                StartCoroutine(GetNextOption(null, 2));
            }
        }

        public void IncorrectOptionChosen()
        {
            SoundController.GetController().PlayFailureSound();
            incorrectAnswers++;

            view.ShowIncorrectAnimation();

            if (currentLevelCounter != 2)
            {
                correctAnswers = 0;
                StartCoroutine(GetNextOption(null, 2.0f));
            }else
            {
                CancelInvoke("RunTime");
                RandomizeStage(2);
            }

            
        }

        private void RandomizeStage(float time)
        {
            int randomStage = Random.Range(0, allStages.Count);
            RaulStage stageToPass;
            stageDictionary.TryGetValue(allStages[randomStage], out stageToPass);
            StartCoroutine(GetNextOption(stageToPass, time));
        }



        private RaulStage GetNextStage()
        {
            currentStage++;
            if (currentStage % allStages.Count == 0 && currentStage!=0)
            {
                return null;
            }
            else
            {
                correctAnswers = 0;
                RaulStage stageToPass;
                stageDictionary.TryGetValue(allStages[currentStage], out stageToPass);
                return stageToPass;
            }
        }

        private void ChangeToNextLevel()
        {
            currentLevelCounter++;
            currentStage = -1;

            foreach (KeyValuePair<Stages, RaulStage> entry in stageDictionary)
            {
                entry.Value.UpdateLevelValues(currentLevelCounter);
            }

            if (currentLevelCounter == 0)
            {
                currentLevel = new RaulLevel1(animalSprites);
                StartCoroutine(GetNextOption(GetNextStage(), 0));
            }
            else if(currentLevelCounter == 1)
            {
                currentLevel = new RaulLevel2(animalSprites);
                StartCoroutine(GetNextOption(GetNextStage(), 2));
            }
            else if(currentLevelCounter == 2)
            {
                currentLevel = new RaulLevel3(animalSprites);
                correctAnswers = 0;

                currentTime = 26;
                view.SetTime(currentTime);
                RandomizeStage(2);
            }else
            {
                RestartGame();
                //ACA TENES QUE POINER EL ENDGAME MARIAAAAAAAAAAAAAAAAAA 
                
            }
            
        }

        private IEnumerator GetNextOption(RaulStage newStage, float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            view.ResetView();

            if (newStage != null)
            {
                currentLevel.SetNewStage(newStage);
            }

            currentLevel.GetNewOption();



            if (currentLevelCounter == 2)
            {
                InvokeRepeating("RunTime", 0, 1f);
            }
        }

        public void RunTime()
        {
            currentTime -= 1;
            if(currentTime < 0)
            {
                CancelInvoke("RunTime");
                ChangeToNextLevel();
            }else
            {
                view.SetTime(currentTime);
            }
        }

        public void RestartGame()
        {
            currentLevelCounter = -1;
            ChangeToNextLevel();
        }


    }
}