using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;

namespace Assets.Scripts.Games.TiteresActivity {
	public class TiteresActivityView : LevelView {
		public Text clock, rules;
		public Button next, previous, okBtn, soundBtn;

		public Image obj, landscape;
		public List<Image> draggers;

		public Randomizer objectLandscapeRandomizer;

		private Sprite[] objects;
		private Material[] landscapes;
		private int currentRule, currentObjectLandscape;
		bool timerActive;
		private TiteresActivityModel model;

		public void Start(){
			model = new TiteresActivityModel();
			objects = Resources.LoadAll<Sprite>("Sprites/TiteresActivity/objects");
			landscapes = Resources.LoadAll<Material>("Sprites/TiteresActivity/Materials");
			objectLandscapeRandomizer = Randomizer.New (landscapes.Length-1);
			Begin();
		}

		public void Begin(){
			ShowExplanation();
		}

		override public void Next(bool first = false){
			if(model.GameEnded()) {
				EndGame(60, 0, 1250);
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
				TimeLevel(model.CurrentLvl());
				model.WithTime();
			} else {
				NormalLevel(model.CurrentLvl());
			}
		}

		void NormalLevel(TiteresLevel lvl) {
			clock.gameObject.SetActive(false);

			SetRule();
		}

		void TimeLevel(TiteresLevel lvl) {
			clock.gameObject.SetActive(true);
			SetClock();
			StartTimer(true);
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
			PlaySoundClick ();
			SetRule();
		}

		public void PreviousClick(){
			currentRule--;
			PlaySoundClick ();
			SetRule();
		}

		void SetRule() {
			List<TiteresDirection> actions = model.CurrentLvl().ActionsToShow();
			rules.text = actions[currentRule].GetText(actions,currentObjectLandscape);
			CheckButtons();
		}

		void CheckButtons() {
			next.interactable = currentRule != (draggers.Count - 1);
			previous.interactable = currentRule != 0;
		}

		public void SoundClick(){
			
		}

		public void OkClick() {
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
				ShowRightAnswerAnimation();
				model.Correct();
				model.CorrectTimer();
				SetClock();
				model.NextLvl();
			} else {
				PlayWrongSound();
				model.Wrong();
				EndGame(60, 0, 1250);
			}
		}

		void NoTimeOkClick() {
			if(IsCorrect()){
				//correct
				ShowRightAnswerAnimation();
				model.Correct();
				model.NextLvl();
			} else {
				PlayWrongSound();
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
			foreach(Image d in draggers) {
				if(!d.GetComponent<TiteresDragger>().IsDroppedInLandscape()){
					okBtn.interactable = false;
					return;
				}
			}
			okBtn.interactable = true;
		}

		public void RestartGame(){
			ResetPuppets();
			Start();
		}
	}
}