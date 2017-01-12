using System;
using Assets.Scripts.App;
using Assets.Scripts.Games;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;
using Assets.Scripts.Common.Dragger;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomActivityView : DraggerView {
		public Image upperBoard;
		public Button soundBtn;
		public GameObject explanationPanel;

		private Sprite[] boards, carpets;
		private JSONArray lvls;
		private int currentLvl;

		public void Start(){
			lvls = JSON.Parse(Resources.Load<Text>("BedroomActivity/bedroom.json").text).AsArray;
			boards = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/consignas");
			Begin();
		}

		public void Begin(){
			explanationPanel.SetActive(true);
			Next(true);
		}

		public void Next(bool first = false){
			if(!first){
				SetCurrentLevel(false);
				currentLvl++;
			}

			if(currentLvl == lvls.Count - 1) EndGame();
			else SetCurrentLevel(true);
		}

		private void EndGame() {
			
		}

		private void SetCurrentLevel(bool enabled) {
			JSONClass lvl = lvls[currentLvl].AsObject;

			switch((StageType) Enum.Parse(typeof(StageType), lvl["type"].Value)){
			case StageType.CLICK:
				SetClick(lvl, enabled);
				break;
			case StageType.TOGGLE:
				SetToggle(lvl, enabled);
				break;
			case StageType.DRAG:
				SetDrag(lvl, enabled);
				break;
			}
		}

		void SetDrag(JSONClass lvl, bool enabled) {
			JSONArray draggers = lvl["object"].AsArray;

			foreach(JSONNode d in draggers) {
				GameObject find = GameObject.Find(d.Value);
				find.GetComponent<DraggerHandler>().SetActive(enabled);
			}
		}

		public void QuitExplanation(){ explanationPanel.SetActive(false); }

		void SetToggle(JSONClass lvl, bool enabled) {
			JSONArray target = lvl["correct"].AsArray;
			JSONArray wrong = lvl["wrong"].AsArray;
			string sound = lvl["sound"].Value;

			foreach(JSONNode t in target) {
				GameObject find = GameObject.Find(t.Value);
				find.GetComponent<Toggle>().enabled = enabled;
			}

			foreach(JSONNode w in wrong) {
				GameObject find = GameObject.Find(w.Value);
				//find.GetComponent<Button>().enabled = enabled;
			}
		}

		void SetClick(JSONClass lvl, bool enabled){
			JSONArray target = lvl["target"].AsArray;
			JSONArray wrong = lvl["wrong"].AsArray;
			string sound = lvl["sound"].Value;

			foreach(JSONNode t in target) {
				GameObject find = GameObject.Find(t.Value);
				find.GetComponent<Button>().enabled = enabled;
			}

			foreach(JSONNode w in wrong) {
				GameObject find = GameObject.Find(w.Value);
				find.GetComponent<Button>().enabled = enabled;
			}

			//TODO sound?
		}

		#region implemented abstract members of DraggerView

		public override void Dropped(DraggerHandler dropped, DraggerSlot where) {
			
		}

		public void MuebleEnter(){
			if(DraggerHandler.itemBeingDragged != null){
				//change mueble frame.
			}
		}

		public void MuebleLeave(){
			
		}

		public override bool CanDropInSlot(DraggerHandler dropper, DraggerSlot slot) {
			return slot.gameObject.name == lvls[currentLvl].AsObject["target"].Value;
		}

		#endregion

		public void ClickTarget(GameObject target){
			target.SetActive(false);
			if(CheckIfFinished()) Next();
		}

		bool CheckIfFinished() {
			JSONArray targets = lvls[currentLvl].AsObject["target"].AsArray;
			foreach(JSONNode t in targets) {
				if(GameObject.Find(t.Value).activeSelf)
					return false;
			}
			return true;
		}

		public void ClickCorrect(){
			Next();
		}

		public void ClickWrong(){

		}

		public override void RestartGame(){
			
		}
	}
}