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

		private Sprite[] objects, landscapes;
		private int currentRule;
		private TiteresActivityModel model;

		public void Start(){
			model = new TiteresActivityModel();
			objects = Resources.LoadAll<Sprite>("Sprites/TiteresActivity/objects");
			landscapes = Resources.LoadAll<Sprite>("Sprites/TiteresActivity/landscapes");
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
			//TODO randomize landscape and object.
			if(model.HasTime()){
				TimeLevel(model.CurrentLvl());
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
		}

		void ResetPuppets() {
			draggers.ForEach((d) => d.GetComponent<TiteresDragger>().SetToInitialPosition());
		}

		public void NextClick(){
			currentRule++;
			SetRule();
		}

		public void PreviousClick(){
			currentRule--;
			SetRule();
		}

		void SetRule() {
			List<TiteresDirection> actions = model.CurrentLvl().Actions();
			rules.text = actions[currentRule].GetText(actions);
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
			
		}

		void NoTimeOkClick() {
			if(IsCorrect()){
				//correct
				PlayRightSound();
				model.Correct();
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
			if(action.action == TiteresAction.RIGHT_ARM && index != 1) {
				Debug.Log("not right arm");
				return false;
			}
			if(action.action == TiteresAction.LEFT_ARM && index != 2) {
				Debug.Log("not left arm");
				return false;
			}
			if(action.action == TiteresAction.SIT && index != 3) {
				Debug.Log("not sitting");
				return false;
			}

			//Check for direction

			Vector2 draggerPosition = dragger.rectTransform.rect.center;

			Debug.Log("dragger x: " + draggerPosition.x);
			Debug.Log("dragger y: " + draggerPosition.y);

			if(action.relativeToPuppetNumber == -1){
				Vector2 landscapePosition = landscape.rectTransform.rect.position;

				Debug.Log("landscape x: " + landscapePosition.x);
				Debug.Log("landscape y: " + landscapePosition.y);

				if(action.direction == Direction.RIGHT && draggerPosition.x < landscapePosition.x) {
					Debug.Log("not right");
					return false;
				}
				if(action.direction == Direction.LEFT && draggerPosition.x > landscapePosition.x) {
					Debug.Log("not left");
					return false;
				}
				if(action.direction == Direction.UP && draggerPosition.y > landscapePosition.y) {
					Debug.Log("not up");
					return false;
				}
				if(action.direction == Direction.DOWN && draggerPosition.y < landscapePosition.y) {
					Debug.Log("not down");
					return false;
				}

			} else {
				Vector2 relativePosition = draggers[action.relativeToPuppetNumber].rectTransform.rect.center;
				//for extra precision in other moment.
				//Vector2 size = draggers[action.relativeToPuppetNumber].rectTransform.rect.size;

				Debug.Log("relative x: " + relativePosition.x);
				Debug.Log("relative y: " + relativePosition.y);

				if(action.direction == Direction.RIGHT && draggerPosition.x < relativePosition.x) {
					Debug.Log("not right");
					return false;
				}
				if(action.direction == Direction.LEFT && draggerPosition.x > relativePosition.x) {
					Debug.Log("not left");
					return false;
				}
				if(action.direction == Direction.UP && draggerPosition.y > relativePosition.y) {
					Debug.Log("not up");
					return false;
				}
				if(action.direction == Direction.DOWN && draggerPosition.y < relativePosition.y) {
					Debug.Log("not down");
					return false;
				}
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