using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Games;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;
using System.Security.Policy;
using Assets.Scripts.Common;
using UnityEngine.EventSystems;
using System.Threading;

public class PlagasActivityView : LevelView {
	public Button okBtn;
	public List<Image> tiles, clocks;
	public Text letter, number;

	private Sprite[] tileSprites;
	private Randomizer veggieRandomizer = Randomizer.New(3, 1), moleRandomizer = Randomizer.New(6, 4);
	private const int GRASS_SPRITE = 0, SMACKED_MOLE_SPRITE = 7;
	private PlagasActivityModel model;

	override public void Next(bool first = false){
		if (!first) PlaySoundClick();
		ClocksActive(false);
		okBtn.enabled = true;
		SetCurrentLevel();
	}

	void ClocksActive(bool active) {
		clocks.ForEach((c) => c.gameObject.SetActive(active));
	}

	public void OkClick() {
		okBtn.enabled = false;
		if (IsCorrect()){
			model.Correct();
			ShowRightAnswerAnimation();
		} else {
			ShowWrongAnswerAnimation();
			model.Wrong();
		}
	}

	bool IsCorrect() {
		

		return true;
	}

	public void Start(){
		model = new PlagasActivityModel();
		tileSprites = Resources.LoadAll<Sprite>("Sprites/PlagasActivity/tiles");
		Begin();
	}

	public void Begin(){
		ShowExplanation();
	}

	private void SetCurrentLevel() {
		CheckOk();
		//deberia ser con herencia, pero odio c# :)
		if(model.HasTime()){
			TimeLevel(model.CurrentLvl());
		} else {
			TimeLevel(model.CurrentLvl());
		}
	}

	int a = 0;

	void TimeLevel(PlagasLevel lvl) {
		Timer timer = null; 
		timer = new System.Threading.Timer((obj) =>
			{
				Debug.Log("Paso un sec");
				a++;
				if(a == 5) timer.Dispose();
			}, null, 1000, System.Threading.Timeout.Infinite);
	}

	void NormalLevel(PlagasLevel lvl) {
		PlaceMoles(lvl.MoleQuantity());
	}

	void PlaceMoles(int moleQuantity) {
		Randomizer tileRandomizer = Randomizer.New(tiles.Count - 1);
		int placedMoles = 0;

		while(placedMoles != moleQuantity){
			int nextSpot = tileRandomizer.Next();
			if(IsSpotFree(nextSpot)){
				tiles[nextSpot].sprite = tileSprites[moleRandomizer.Next()];
			}
		}
	}

	bool IsSpotFree(int spot) {
		return tiles[spot].sprite != tileSprites[GRASS_SPRITE];
	}

	public void LetterClick(string l){
		letter.text = l;
		CheckOk();
	}

	public void NumberClick(string n){
		number.text = n;
		CheckOk();
	}

	void CheckOk() {
		okBtn.interactable = CanSubmit();
	}

	bool CanSubmit() {
		return letter.text.Length == 1 && number.text.Length == 1;
	}
}
