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
using System;

public class PlagasActivityView : LevelView {
	public Button okBtn;
	public List<Image> tiles, clocks;
	public Text letter, number;

	private Sprite[] tileSprites;
	private Randomizer veggieRandomizer = Randomizer.New(3, 1), moleRandomizer = Randomizer.New(6, 4);
	private const int GRASS_SPRITE = 0, SMACKED_MOLE_SPRITE = 7;
	private PlagasActivityModel model;

	Timer timer = null;

	override public void Next(bool first = false){
		if (!first) PlaySoundClick();
		ClocksActive(false);
		ResetTiles();
		letter.text = "";
		number.text = "";
		SetCurrentLevel();
		CheckOk();
	}

	void ClocksActive(bool active) {
		clocks.ForEach((c) => c.gameObject.SetActive(active));
	}

	public void OkClick() {
		int row = model.GetRow(number.text);
		int column = model.GetColumn(letter.text);

		if(model.HasTime()){
			TimeOkClick(row, column);
		} else {
			NoTimeOkClick(row, column);
		}
	}

	void ResetTiles() {
		foreach(var tile in tiles) {
			tile.sprite = tileSprites[GRASS_SPRITE];
		}
	}

	void NoTimeOkClick(int row, int column) {
		int slot = model.GetSlot(row, column);
		if(tiles[slot].sprite != tileSprites[GRASS_SPRITE] && tiles[slot].sprite != tileSprites[SMACKED_MOLE_SPRITE]){
			//correct
			model.Correct();
			SmackMole(slot);

			CheckEndLevel();
		} else {
			PlayWrongSound();
		}
	}

	void TimeOkClick(int row, int column) {
		if (model.IsCorrectTime(row, column)){
			model.Correct();
			PlayRightSound();
			SmackMole(model.GetSlot(row, column));
			//ShowRightAnswerAnimation();

			CheckEndLevel();
		} else {
			//ShowWrongAnswerAnimation();
			PlayWrongSound();
			model.Wrong();
		}
	}

	void SmackMole(int slot) {
		tiles[slot].sprite = tileSprites[SMACKED_MOLE_SPRITE];
	}

	void CheckEndLevel() {
		if(model.IsLevelEnded()){
			if(timer != null){
				timer.Dispose();
				timer = null;
			}
			ShowRightAnswerAnimation();
			model.NextLvl();
		}
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
			NormalLevel(model.CurrentLvl());
		}
	}

	void TimeLevel(PlagasLevel lvl) {
		int randomSpawn = lvl.RandomSpawnTime();
		int moleQuantity = lvl.MolesInSpawn(randomSpawn);

		for(int i = 0; i < moleQuantity; i++) {
			int freeSlot = model.GetFreeSlot();

			model.SetTimerTile(freeSlot, randomSpawn);
		}

		StartTimer(() => TimerFunction(true));
	}

	void StartTimer(Action action) {
		timer = new System.Threading.Timer((a) => action.Invoke(), null, 1000, System.Threading.Timeout.Infinite);
	}

	public void TimerFunction(bool first = false) {
		Debug.Log("segundo");

		UpdateView();

		if(timer != null) StartTimer(() => TimerFunction());
	}

	void UpdateView() {
		model.DecreaseTimer();
		List<PlagaTile> t = model.GetTiles();

		for(int i = 0; i < t.Count; i++) {
			PlagaTile tile = t[i];
			clocks[i].GetComponentInChildren<Text>(true).text = tile.GetTimer().ToString();

			//si tiene que aparecer un vegetal.
			if(tile.HasToAppear()){
				tiles[i].sprite = tileSprites[veggieRandomizer.Next()];
				t[i].AppearVeggie();
			}
			//si se termino el tiempo y no le pego.
			if(tile.TimeDoneAndNotSmacked()){
				tiles[i].sprite = tileSprites[GRASS_SPRITE];
				clocks[i].gameObject.SetActive(false);
				t[i].DissapearMole();
			}
			//si sale vegetal y aparece topo.
			if(tile.MoleHasToAppear()){
				tiles[i].sprite = GetMoleFromVeggie(tiles[i].sprite);
				t[i].AppearMole();
			}
		}
	}

	Sprite GetMoleFromVeggie(Sprite sprite) {
		return tileSprites[Array.IndexOf(tileSprites, sprite) + 3];
	}

	void NormalLevel(PlagasLevel lvl) {
		PlaceMoles(lvl.MoleQuantity());
	}

	void PlaceMoles(int moleQuantity) {
		for(int i = 0; i < moleQuantity; i++) {
			int nextSpot = model.GetFreeSlot();
			tiles[nextSpot].sprite = tileSprites[moleRandomizer.Next()];
		}
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