using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games
{
    public class RaulSaysView : LevelView
    {
        [SerializeField]
        GameObject image8Task;

        [SerializeField]
        GameObject image4Result;

        [SerializeField]
        GameObject image8Result;

        [SerializeField]
        Image firstArrowIndicator;
        [SerializeField]
        Image secondArrowIndicator;

        [SerializeField]
        GameObject wordHolder;

        Text[] words;

        [SerializeField]
        Sprite[] raulStates;

        [SerializeField]
        private Image raulImage;

        private List<GameObject> taskImages;

        private GameObject resultToModify;

        public Text time;
        public GameObject timeLogo;
        // Use this for initialization
        void Start()
        {
            words = new Text[2];

            words[0] = wordHolder.transform.GetChild(0).gameObject.GetComponent<Text>();
            words[1] = wordHolder.transform.GetChild(1).gameObject.GetComponent<Text>();
        }

        

        public void SetLevel1()
        {
            image8Result.SetActive(false);
            image4Result.SetActive(true);
            resultToModify = image4Result;
            taskImages = new List<GameObject>();

            for (int i = 0; i < image8Task.transform.childCount; i++)
            {
                if (i < 4)
                {
                    taskImages.Add(image8Task.transform.GetChild(i).gameObject);
                }
                else
                {
                    image8Task.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

        }

        public void SetLevel2()
        {

            taskImages = new List<GameObject>();
            image8Result.SetActive(true);
            image4Result.SetActive(false);
            resultToModify = image8Result;
            for (int i = 4; i < image8Task.transform.childCount; i++)
            {
                image8Task.transform.GetChild(i).gameObject.SetActive(true);
                taskImages.Add(image8Task.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < image8Task.transform.childCount/2; i++)
            {
                taskImages.Add(image8Task.transform.GetChild(i).gameObject);
            }

        }

        public void SetLevel3()
        {

            taskImages = new List<GameObject>();
            image8Result.SetActive(true);
            image4Result.SetActive(false);
            time.gameObject.SetActive(true);
            timeLogo.SetActive(true);
            time.text = "25 s";
            resultToModify = image8Result;
            for (int i = 0; i < image8Task.transform.childCount; i++)
            {
                image8Task.transform.GetChild(i).gameObject.SetActive(true);
                taskImages.Add(image8Task.transform.GetChild(i).gameObject);
            }



        }

        public void SetArrowStage()
        {
            wordHolder.SetActive(false);
            firstArrowIndicator.gameObject.SetActive(true);
            secondArrowIndicator.gameObject.SetActive(true);
            firstArrowIndicator.color = Color.clear;
            secondArrowIndicator.color = Color.clear;

        }

        public void ShowWordOption(int answer, string[] wordToShow, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
        {
            words[0].text = wordToShow[0];
            words[1].text = "";
            if (wordToShow.Length > 1)
            {
                words[1].text = wordToShow[1];
            }
            SetOptions(answer, restAnimalEnunciado, restAnimalResultado);

        }

        public void ShowArrowOption(int answer, Sprite[] spriteToShow, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
        {
            firstArrowIndicator.color = Color.white;
            firstArrowIndicator.sprite = spriteToShow[0];

            if (spriteToShow.Length > 1)
            {
                secondArrowIndicator.color = Color.white;
                secondArrowIndicator.sprite = spriteToShow[1];
            }


            SetOptions(answer, restAnimalEnunciado, restAnimalResultado);

        }

        public void SetWordStage()
        {
            wordHolder.SetActive(true);
            if (words == null)
            {
                words = new Text[2];
                words[0] = wordHolder.transform.GetChild(0).gameObject.GetComponent<Text>();
                words[1] = wordHolder.transform.GetChild(1).gameObject.GetComponent<Text>();
            }
            words[0].text = "";
            words[1].text = "";
        }

        public void SetOptions(int answer, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
        {


            for (int i = 0; i < restAnimalEnunciado.Length; i++)
            {

                resultToModify.transform.GetChild(i).gameObject.GetComponent<RaulButton>().isCorrect = (i == answer);
                resultToModify.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = restAnimalResultado[i];

                taskImages[i].GetComponent<Image>().sprite = restAnimalEnunciado[i];

            }

            ShufflePositions(resultToModify);
        }


        public void ShufflePositions(GameObject resultToModify)
        {

            System.Random _random = new System.Random();

            Transform myGO;

            int n = resultToModify.transform.childCount;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                myGO = resultToModify.transform.GetChild(r);
                resultToModify.transform.GetChild(i).SetSiblingIndex(r);
                myGO.SetSiblingIndex(i);
            }


        }

        public void SetTime(int currentTime)
        {
            time.text = currentTime.ToString();
        }

        public void SetAudioStage()
        {
            firstArrowIndicator.gameObject.SetActive(false);
            secondArrowIndicator.gameObject.SetActive(false);
            wordHolder.SetActive(false);
        }

        public void ShowCorrectAnimation()
        {
            ChangeButtonState(false);
            raulImage.sprite = raulStates[1];
        }

        public void ShowIncorrectAnimation()
        {
            ChangeButtonState(false);
            raulImage.sprite = raulStates[2];
        }

        public void ResetView()
        {
            ChangeButtonState(true);
            raulImage.sprite = raulStates[0];
        }

        private void ChangeButtonState(bool state)
        {
            for (int i = 0; i < resultToModify.transform.childCount; i++)
            {
                resultToModify.transform.GetChild(i).gameObject.GetComponent<Button>().enabled = state;
            }
        }

        public override void Next(bool first = false)
        {
            throw new NotImplementedException();
        }
    }
}
