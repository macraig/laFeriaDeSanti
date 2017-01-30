using Assets.Scripts.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                ChangeToNextStage(2);
            }else if(currentLevelCounter == 2)
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
                Invoke("GetNextOption", 2.0f);
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
                Invoke("GetNextOption", 2.0f);
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
            currentLevel.SetNewStage(stageToPass);
            Invoke("GetNextOption", time);
        }

        private void ChangeToNextStage(float time)
        {
            currentStage++;
            if (currentStage % allStages.Count == 0 && currentStage!=0)
            {
                ChangeToNextLevel();
            }
            else
            {
                correctAnswers = 0;
                RaulStage stageToPass;
                stageDictionary.TryGetValue(allStages[currentStage], out stageToPass);
                currentLevel.SetNewStage(stageToPass);
                Invoke("GetNextOption", time);
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
                ChangeToNextStage(0);
            }
            else if(currentLevelCounter == 1)
            {
                currentLevel = new RaulLevel2(animalSprites);
                ChangeToNextStage(0);
            }
            else if(currentLevelCounter == 2)
            {
                correctAnswers = 0;
                currentLevel = new RaulLevel3(animalSprites);
                currentTime = 26;
                view.SetTime(currentTime);
                RandomizeStage(0);
            }
            
        }

        private void GetNextOption()
        {
            view.ResetView();
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
                StopCoroutine("RunTime");
                ChangeToNextLevel();
            }else
            {
                view.SetTime(currentTime);
            }
        }


    }
}
