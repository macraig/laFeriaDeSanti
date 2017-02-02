using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;

namespace Assets.Scripts.Games.TiteresActivity {
	public class TiteresActivityView : LevelView {
		public Text clock, rules;
		public Button next, previous, okBtn, soundBtn;

		public Image obj;
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

		public void OkClick(){

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