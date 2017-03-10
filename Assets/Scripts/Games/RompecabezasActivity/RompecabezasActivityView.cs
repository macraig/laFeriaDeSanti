using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Games;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;


namespace Assets.Scripts.Games.RompecabezasActivity {
	public class RompecabezasActivityView : LevelView {
		public Text clock, timeCounter;
		public Button okBtn;
		public Image lampImage,puppetImage;
		public GameObject clockPlaca;

		public List<Image> tiles;
		public List<Part> draggers;

		bool timerActive,switchTime;
		private int timeAnswers;
		private List<Sprite> parts,transparentParts,lamps,puppetSprites;
		private int lampIndex;
		private RompecabezasActivityModel model;

		public const int EMPTY_TILE = 26;

		public void Start(){
			model = new RompecabezasActivityModel();
			parts = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/RompecabezasActivity/tiles"));
			transparentParts = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/RompecabezasActivity/tiles2"));
			lamps = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/RompecabezasActivity/lamps"));
			puppetSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/RompecabezasActivity/puppet"));
			timeAnswers = 0;
			switchTime = true;

			Begin();
		}

		public void Begin(){
			ShowExplanation();
		}

		override public void Next(bool first = false){
			if(model.GameEnded()) {
				EndGame(60, 0, 1250);
			} else {
				puppetImage.sprite = puppetSprites [0];
				lampIndex = Randomizer.RandomInRange (3)*2;
				lampImage.sprite = lamps [lampIndex];
				ResetTiles();
				SetCurrentLevel();
			}
		}

		void SetCurrentLevel() {
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
			base.OnNextLevelAnimationEnd ();
			PlayTimeLevelMusic ();
			TimeLevel(model.CurrentLvl());
		}

		void NormalLevel(RompecabezasLevel lvl) {
			clockPlaca.SetActive(false);
			SetStartParts(lvl.StartParts());
			SetEndParts(lvl.EndParts());

			SetDraggers(lvl.DraggerParts());
		}

		void SetDraggers(List<PartModel> draggerParts) {
			for(int i = 0; i < draggers.Count; i++) {
				draggers[i].gameObject.SetActive(i < draggerParts.Count);
				if(i < draggerParts.Count){
					draggers[i].SetModel(draggerParts[i], parts);
				}
			}

		}

		void SetEndParts(List<PartModel> parts) {
			foreach(PartModel end in parts) {
				tiles[TileNumber(end.row, end.col)].GetComponent<RompecabezasSlot>().SetEnd(end);
			}
		}

		void SetStartParts(List<PartModel> parts) {
			foreach(PartModel start in parts) {
				tiles[TileNumber(start.row, start.col)].GetComponent<RompecabezasSlot>().SetStart(start);
			}
		}

		public int TileNumber(int row, int col){
			return (row * RompecabezasLevel.GRID) + col;
		}

		void TimeLevel(RompecabezasLevel lvl) {
			clockPlaca.SetActive(true);
			SetClock();
			StartTimer(true);

			SetStartParts(lvl.StartParts());
			SetEndParts(lvl.EndParts());

			SetDraggers(lvl.DraggerParts());
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

		void ResetTiles() {
			tiles.ForEach(t => {
				t.GetComponent<RompecabezasSlot>().Reset();
				t.GetComponent<RompecabezasSlot>().EndSlot(false);
				t.GetComponent<RompecabezasSlot>().StartSlot(false);
			});
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
				lampImage.sprite = lamps[lampIndex+1];
				puppetImage.sprite = puppetSprites [1];
				model.NextLvl();
				ShowRightAnswerAnimation();
				timeAnswers++;
				timeCounter.text = timeAnswers.ToString ();
//				model.Correct();
				SetClock();
			} else {
				puppetImage.sprite = puppetSprites [2];
				ShowWrongAnswerAnimation ();
//				model.Wrong();

			}
		}

		void NoTimeOkClick() {
			if(IsCorrect()){
				//correct
				lampImage.sprite = lamps[lampIndex+1];
				puppetImage.sprite = puppetSprites [1];
				ShowRightAnswerAnimation();
				model.Correct();
				model.NextLvl();
			} else {
				puppetImage.sprite = puppetSprites [2];
				ShowWrongAnswerAnimation ();
				model.Wrong();
			}
		}

		bool IsCorrect() {
			List<PartModel> startParts = model.CurrentLvl().StartParts();

			foreach(PartModel startPart in startParts) {
				int newRow = startPart.row;
				int newCol = startPart.col;

				Direction dir = startPart.direction;

				while(true){
					newRow = DirectionPlusRow(dir, newRow);
					newCol = DirectionPlusCol(dir, newCol);

					RompecabezasSlot slot = tiles[TileNumber(newRow, newCol)].GetComponent<RompecabezasSlot>();

					if(slot.IsEnd()) break;

					Part currentPart = slot.GetCurrent();

					if(currentPart == null) return false;

					PartModel m = currentPart.Model();
					if(m.isDouble){
						if(!m.isCross){
							dir = NotCrossDouble(dir, m.isLeftUp);
						}
					} else {
						dir = GetNextDirection(m, dir);

						if(dir == Direction.NULL) return false;
					}
				}
			}
			
			return true;
		}

		Direction GetNextDirection(PartModel m, Direction dir) {
			

			//if(m.direction == m.previousDir && (m.direction == dir || m.direction == OppositeDir(dir))) return dir;

			if(m.previousDir == dir)
				return m.direction;
			else if(OppositeDir(m.direction) == dir)
				return OppositeDir(m.previousDir);
			else
				return Direction.NULL;
		}

		Direction OppositeDir(Direction direction) {
			if(direction == Direction.UP) return Direction.DOWN;
			if(direction == Direction.DOWN) return Direction.UP;
			if(direction == Direction.LEFT) return Direction.RIGHT;
			if(direction == Direction.RIGHT) return Direction.LEFT;
			return Direction.NULL;
		}

		Direction NotCrossDouble(Direction dir, bool isLeftUp) {
			if(dir == Direction.RIGHT) return isLeftUp ? Direction.UP : Direction.DOWN;
			if(dir == Direction.LEFT) return isLeftUp ? Direction.DOWN : Direction.UP;
			if(dir == Direction.UP) return isLeftUp ? Direction.RIGHT : Direction.LEFT;
			if(dir == Direction.DOWN) return isLeftUp ? Direction.LEFT : Direction.RIGHT;
			return Direction.NULL;
		}

		int DirectionPlusRow(Direction d, int row) {
			if(d == Direction.UP) return row - 1;
			if(d == Direction.DOWN) return row + 1;
			return row;
		}

		int DirectionPlusCol(Direction d, int col) {
			if(d == Direction.LEFT) return col - 1;
			if(d == Direction.RIGHT) return col + 1;
			return col;
		}

		public int GetSpriteIndex (Sprite sprite)
		{
			return parts.IndexOf (sprite);
		}

		public Sprite PartSprite(int index){
			return parts[index];
		}

		override public void RestartGame(){
			base.RestartGame ();
			ResetTiles();
			Start();
		}

		override public void OnWrongAnimationEnd(){
			puppetImage.sprite = puppetSprites [0];
			base.OnWrongAnimationEnd ();
			if(model.HasTime())EndGame(0, 0, 1250);
		}

		override public void EnableComponents(bool enable){
			base.EnableComponents (enable);
			okBtn.interactable = enable;
		}

	}
}