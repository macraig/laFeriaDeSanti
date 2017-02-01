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

		public Sprite[] objects, landscapes;
		public Randomizer objectLandscapeRandomizer;

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
			if(model.HasTime()){
				TimeLevel(model.CurrentLvl());
			} else {
				NormalLevel(model.CurrentLvl());
			}
		}

		void NormalLevel(TiteresLevel lvl) {
			
		}

		void TimeLevel(TiteresLevel lvl) {
			
		}

		void ResetPuppets() {
			
		}

		public void NextClick(){
			
		}

		public void PreviousClick(){

		}

		public void SoundClick(){
			
		}

		public void OkClick(){

		}

		public void CheckOk(){
			
		}

		public void RestartGame(){
			Start();
		}
	}
}