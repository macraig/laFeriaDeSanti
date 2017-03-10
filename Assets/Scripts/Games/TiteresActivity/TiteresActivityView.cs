using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.TiteresActivity {
	public class TiteresActivityView : LevelView {
		public Text clock, rules, tickets;
		public Image clockImage;
		public Button next, previous, okBtn, soundBtn;

		public Image obj, landscape;
		public List<Image> draggers;
		public Image puppetsCharacter;

		public Randomizer objectLandscapeRandomizer;

		private Sprite[] objects, characterSprites;
		private Material[] landscapes;
		private List<AudioClip> audios;
		private int currentRule, currentObjectLandscape;
		bool timerActive,switchTime;

		private TiteresActivityModel model;


		public void Start(){
			model = new TiteresActivityModel();
			tickets.text = model.Counter.ToString ();
			objects = Resources.LoadAll<Sprite>("Sprites/TiteresActivity/objects");
			characterSprites = Resources.LoadAll<Sprite>("Sprites/TiteresActivity/puppetWinLose");
			landscapes = Resources.LoadAll<Material>("Sprites/TiteresActivity/Materials");
			menuBtn.interactable = true;
			objectLandscapeRandomizer = Randomizer.New (landscapes.Length-1);
			switchTime = true;
			Begin();
		}

		public void Begin(){
			ShowExplanation();
		}

		public override void RestartGame(){
			base.RestartGame ();
			ResetPuppets();
			Start();
		}

		override public void ShowInGameMenu(){
			base.ShowInGameMenu ();
			SoundController.GetController ().SetConcatenatingAudios (false);
			soundBtn.interactable = true;
		}

		override public void Next(bool first = false){
			if(model.GameEnded()) {
				EndGame(0, 0, 800);
			} else {
				

				ResetPuppets();
				SetCurrentLevel();
				CheckOk();
			}
		}

		void SetCurrentLevel() {
			currentRule = 0;
			currentObjectLandscape = objectLandscapeRandomizer.Next();
			landscape.material = landscapes[currentObjectLandscape];
			obj.sprite = objects [currentObjectLandscape];

			if(model.HasTime()){
				if (switchTime) {
					Invoke("ShowNextLevelAnimation",0.2f) ;
					switchTime = false;
				} else {
					TimeLevel(model.CurrentLvl());
				}

//				menuBtn.interactable = false;


			} else {
				NormalLevel(model.CurrentLvl());
			}
		}

		override public void OnNextLevelAnimationEnd(){
			PlayTimeLevelMusic ();
//			menuBtn.interactable = false;
			TimeLevel(model.CurrentLvl());
		}

		void NormalLevel(TiteresLevel lvl) {
			clock.gameObject.SetActive(false);
			clockImage.gameObject.SetActive(false);
			SetRule();
		}

		void TimeLevel(TiteresLevel lvl) {
			clock.gameObject.SetActive(true);
			clockImage.gameObject.SetActive(true);
			SetClock();
			StartTimer(true);
			SetRule ();
		}

		void StartTimer(bool first = false) {
			StartCoroutine(TimerFunction(first));
			timerActive = true;
		}

		public IEnumerator TimerFunction(bool first = false) {
			yield return new WaitForSeconds(1);
			Debug.Log("segundo");

			UpdateView();

			if(timerActive) StartTimer();
		}

		void SetClock() {
			clock.text = model.GetTimer().ToString();
		}

		void UpdateView() {
			model.DecreaseTimer();

			SetClock();

			if(model.IsTimerDone()){
				timerActive = false;
				EndGame(60, 0, 1250);
			}
		}

		void ResetPuppets() {
			foreach (Image dragger in draggers) {
				dragger.GetComponent<TiteresDragger>().SetToInitialPosition ();
				dragger.GetComponent<TiteresDragger>().ResetPuppetSprite ();
			}
		}

		public void NextClick(){
			currentRule++;
			SoundController.GetController ().SetConcatenatingAudios (false);
			soundBtn.interactable = true;
			PlaySoundClick ();
			SetRule();
		}

		public void PreviousClick(){
			currentRule--;
			SoundController.GetController ().SetConcatenatingAudios (false);
			soundBtn.interactable = true;
			PlaySoundClick ();
			SetRule();
		}

		void SetRule() {
			List<TiteresDirection> actions = model.CurrentLvl().ActionsToShow();
			rules.text = actions[currentRule].GetText(actions,currentObjectLandscape);
			audios = actions[currentRule].GetAudios(actions,currentObjectLandscape,model.GetPuppetAudios(),
				model.GetPuppetEndAudios(),model.GetPositionAudios(),model.GetObjectAudios());

			CheckButtons();
		}
			

		void CheckButtons() {
			next.interactable = currentRule != (draggers.Count - 1);
			previous.interactable = currentRule != 0;
		}

		public void SoundClick(){
			soundBtn.interactable = false;
			SoundController.GetController ().ConcatenateAudios (audios,EndSoundMethod);
		}

		public void EndSoundMethod(){
			soundBtn.interactable = true;
		}

		public void OkClick() {
			okBtn.interactable = false;
			if(model.HasTime()){
				TimeOkClick();
			} else {
				NoTimeOkClick();
			}
		}

		void TimeOkClick() {
			timerActive = false;
			if(IsCorrect()){
				//correct
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				model.Counter++;
				tickets.text = model.Counter.ToString();
				ShowRightAnswerAnimation();
				model.CorrectTimer();
				SetClock();
			} else {
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				puppetsCharacter.sprite = characterSprites [1];

				EndGame(0, 0, 800);
			}
		}

		void NoTimeOkClick() {
			if(IsCorrect()){
				//correct
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				model.Correct();
				model.Counter++;
				tickets.text = model.Counter.ToString();
				ShowRightAnswerAnimation();

			} else {
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				ShowWrongAnswerAnimation ();
				model.Wrong();

			}
		}

		bool IsCorrect() {
			List<TiteresDirection> actions = model.CurrentLvl().Actions();

			for(int i = 0; i < actions.Count; i++) {
				TiteresDirection action = actions[i];
				Image dragger = draggers[i];

				if(!IsDirectionCorrect(action, dragger)) return false;
			}
			return true;
		}

		bool IsDirectionCorrect(TiteresDirection action, Image dragger) {
			List<Sprite> puppets = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/TiteresActivity/puppets"));
			int index = puppets.IndexOf(dragger.sprite) % 4;

			//Check for action.
			if(action.action == TiteresAction.STANDING && index != 0) {
				Debug.Log("not standing");
				return false;
			}
			if(action.action == TiteresAction.RIGHT_ARM && index != 2) {
				Debug.Log("not right arm");
				return false;
			}
			if(action.action == TiteresAction.LEFT_ARM && index != 1) {
				Debug.Log("not left arm");
				return false;
			}
			if(action.action == TiteresAction.SIT && index != 3) {
				Debug.Log("not sitting");
				return false;
			}

			//Check for direction

			Vector2 draggerPosition = dragger.transform.position;

			Debug.Log("dragger x: " + draggerPosition.x);
			Debug.Log("dragger y: " + draggerPosition.y);

			Vector2 objPosition = action.relativeToPuppetNumber == -1 ? obj.transform.position : draggers[action.relativeToPuppetNumber].transform.position;

			Debug.Log("obj x: " + objPosition.x);
			Debug.Log("obj y: " + objPosition.y);

			if(action.direction == Direction.RIGHT && draggerPosition.x < objPosition.x) {
				Debug.Log("not right");
				return false;
			}
			if(action.direction == Direction.LEFT && draggerPosition.x > objPosition.x) {
				Debug.Log("not left");
				return false;
			}
			if(action.direction == Direction.UP && draggerPosition.y < objPosition.y) {
				Debug.Log("not up");
				return false;
			}
			if(action.direction == Direction.DOWN && draggerPosition.y > objPosition.y) {
				Debug.Log("not down");
				return false;
			}

			return true;
		}

		public void CheckOk(){
			bool isOk = true;

			foreach(Image d in draggers) {
				if(!d.GetComponent<TiteresDragger>().IsDroppedInLandscape()){
					isOk = false;
					break;
				}
			}
			okBtn.interactable = isOk;

		}

	

		override public void ShowRightAnswerAnimation(){
			base.ShowRightAnswerAnimation ();
			puppetsCharacter.sprite = characterSprites [2];
		}

		override public void ShowWrongAnswerAnimation(){
			base.ShowWrongAnswerAnimation ();
			puppetsCharacter.sprite = characterSprites [1];
		}

		override public void OnRightAnimationEnd(){
			model.NextLvl();
			base.OnRightAnimationEnd ();
			puppetsCharacter.sprite = characterSprites [0];
		}

		override public void OnWrongAnimationEnd(){
			base.OnWrongAnimationEnd ();
			okBtn.interactable = true;
			puppetsCharacter.sprite = characterSprites [0];
		}

	}
}