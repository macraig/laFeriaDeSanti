using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;

namespace Assets.Scripts.Games
{
	public class RaulSaysView : LevelView
    {
		#region implemented abstract members of LevelView

		public override void Next(bool first = false) {
			throw new NotImplementedException();
		}

		#endregion

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
        GameObject leftArmGroup;
        [SerializeField]
        GameObject rightArmGroup;


        [SerializeField]
        GameObject wordHolder;
		[SerializeField]
		Text timeCounter;

        Text[] words;

        [SerializeField]
        Sprite[] raulStates;

        [SerializeField]
        private Image raulImage;

        private List<GameObject> taskImages;

        private GameObject resultToModify;

        public Text time;
        public GameObject timeLogo;

        public GameObject clock;
        public GameObject repeatSoundButton;
		public GameObject placaClock,placaSound;
		private AudioClip signSound;

        // Use this for initialization
        void Start()
        {
            
			words = new Text[2];

            words[0] = wordHolder.transform.GetChild(0).gameObject.GetComponent<Text>();
            words[1] = wordHolder.transform.GetChild(1).gameObject.GetComponent<Text>();
			signSound = Resources.Load<AudioClip> ("Audio/RaulActivity/sign");

        }

		override public void HideExplanation(){
			PlaySoundClick ();
			explanationPanel.SetActive (false);
			menuBtn.enabled = true;
		}

        public void SetLevel1()
        {
			
			placaClock.SetActive (false);
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
//			menuBtn.interactable = false;

			placaClock.SetActive (true);
            taskImages = new List<GameObject>();
            image8Result.SetActive(true);
            image4Result.SetActive(false);
            time.gameObject.SetActive(true);
            timeLogo.SetActive(true);
            time.text = "25";

            resultToModify = image8Result;
            for (int i = 0; i < image8Task.transform.childCount; i++)
            {
                image8Task.transform.GetChild(i).gameObject.SetActive(true);
                taskImages.Add(image8Task.transform.GetChild(i).gameObject);
            }

			PlayTimeLevelMusic ();




        }

        public void SetArrowStage()
        {
			placaSound.SetActive(false);
            wordHolder.SetActive(false);
            firstArrowIndicator.gameObject.SetActive(true);
            secondArrowIndicator.gameObject.SetActive(true);
            firstArrowIndicator.color = Color.clear;
            secondArrowIndicator.color = Color.clear;

        }

        public void ShowWordOption(int answer, string[] wordToShow, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
        {
			PlaySignSound ();
			firstArrowIndicator.color = Color.clear;
            secondArrowIndicator.color = Color.clear;


            if (!wordHolder.activeSelf)
            {
                wordHolder.SetActive(true);

            }
            SetNeutralArms();

            leftArmGroup.transform.eulerAngles = new Vector3(0, 0, 60);
            StartCoroutine(Rotate(leftArmGroup, new Vector3(0, 0, -60), 0.3f, 0));


			if (wordToShow.Length > 1) {
				bool randomBool = Randomizer.RandomBoolean ();
				words[0].text = wordToShow[randomBool ? 0 : 1];
				words [1].text = wordToShow [randomBool ? 1 : 0];

			} else {

				words[0].text = wordToShow[0];
				words[1].text = "";
			}
            SetOptions(answer, restAnimalEnunciado, restAnimalResultado);

        }

        public void ShowSoundOption(int randomResult, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
        {

            SetNeutralArms();
            StartCoroutine(Rotate(leftArmGroup, new Vector3(0, 0, 60), 0.3f, 0));
            StartCoroutine(Rotate(leftArmGroup, new Vector3(0, 0, -60), 0.3f, 0.3f));

            StartCoroutine(Rotate(rightArmGroup, new Vector3(0, 0, -60), 0.3f, 0));
            StartCoroutine(Rotate(rightArmGroup, new Vector3(0, 0, 60), 0.3f, 0.3f));

            RaulSaysController.instance.view.SetOptions(randomResult, restAnimalEnunciado, restAnimalResultado);

        }

        public void ShowArrowOption(int answer, Sprite[] spriteToShow, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
        {
			PlaySignSound ();

            SetNeutralArms();

			firstArrowIndicator.color = Color.white;
			firstArrowIndicator.sprite = spriteToShow[0];
            leftArmGroup.transform.eulerAngles = new Vector3(0, 0, 60);
            StartCoroutine(Rotate(leftArmGroup, new Vector3(0, 0, -60), 0.3f,0));

			if (spriteToShow.Length > 1) {
				bool randomBool = Randomizer.RandomBoolean ();
				firstArrowIndicator.sprite = spriteToShow[randomBool ? 0 : 1];

				secondArrowIndicator.sprite = spriteToShow [randomBool ? 1 : 0];
				secondArrowIndicator.color = Color.white;
				rightArmGroup.transform.eulerAngles = new Vector3 (0, 0, -60);
				StartCoroutine (Rotate (rightArmGroup, new Vector3 (0, 0, 60), 0.3f, 0));
			} 


            SetOptions(answer, restAnimalEnunciado, restAnimalResultado);

        }

        public void SetWordStage()
        {
			placaSound.SetActive(false);
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
			placaSound.SetActive (true);
            firstArrowIndicator.gameObject.SetActive(false);
            secondArrowIndicator.gameObject.SetActive(false);
            wordHolder.SetActive(false);
        }

        public void ShowCorrectAnimation()
        {
            ChangeButtonState(false);

            SetNeutralArms();

            StartCoroutine(Rotate(leftArmGroup, new Vector3(0, 0, -60), 0.2f, 0));
            StartCoroutine(Rotate(leftArmGroup, new Vector3(0, 0, 60), 0.2f, 0.2f));

            StartCoroutine(Rotate(rightArmGroup, new Vector3(0, 0, 60), 0.2f, 0));
            StartCoroutine(Rotate(rightArmGroup, new Vector3(0, 0, -60), 0.2f, 0.2f));

            raulImage.sprite = raulStates[1];
        }

        public void ShowIncorrectAnimation()
        {
            ChangeButtonState(false);

            SetNeutralArms();
            StartCoroutine(Rotate(leftArmGroup, new Vector3(0, 0, 60), 0.2f, 0));
            StartCoroutine(Rotate(rightArmGroup, new Vector3(0, 0, -60), 0.2f, 0));


            raulImage.sprite = raulStates[2];
        }

        private void SetNeutralArms()
        {
            leftArmGroup.transform.eulerAngles = new Vector3(0, 0, 0);
            rightArmGroup.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        public void ResetView()
        {
            ChangeButtonState(true);

            //leftArmGroup.transform.eulerAngles = new Vector3(0, 0, 0);
            //rightArmGroup.transform.eulerAngles = new Vector3(0, 0, 0);
            raulImage.sprite = raulStates[0];
        }

        private void ChangeButtonState(bool state)
        {
            for (int i = 0; i < resultToModify.transform.childCount; i++)
            {
                resultToModify.transform.GetChild(i).gameObject.GetComponent<Button>().enabled = state;
            }
        }

      

        IEnumerator Rotate(GameObject objectToRotate, Vector3 byAngles, float inTime, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            var fromAngle = objectToRotate.transform.rotation;
            var toAngle = Quaternion.Euler(objectToRotate.transform.eulerAngles + byAngles);
            for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
            {
                objectToRotate.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
                yield return null;
            }
            objectToRotate.transform.rotation = toAngle;
        }

		private void PlaySignSound(){
			SoundController.GetController ().PlayClip (signSound);
		}

		public void RefreshCorrectCounter(int correctAnswers){
			timeCounter.text = correctAnswers.ToString();
		}

		override public void EnableComponents(bool enable){
			Debug.Log ("No enabled components");
//				menuBtn.interactable = enable;
			//			soundBtn.interactable = enable;
		}
    }
}
