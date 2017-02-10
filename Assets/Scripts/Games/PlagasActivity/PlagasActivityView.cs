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
	public List<GameObject> lives;
	public Text letter, number, topoCounter;

	private Sprite[] tileSprites;
	private bool keyboardActive;
	private Randomizer veggieRandomizer = Randomizer.New(3, 1), moleRandomizer = Randomizer.New(6, 4);
	private const int GRASS_SPRITE = 0, SMACKED_MOLE_SPRITE = 7, GRASS_HOLE_SPRITE = 8, MOLE_HOLE_SPRITE = 9;

	private AudioClip whackSound, whackMoleSound;
	private PlagasActivityModel model;
	private bool switchTime = true;
	bool timerActive = false;
	public GameObject placaTopos;

	override public void Next(bool first = false){
		if(model.GameEnded()) {
			EndGame(60, 0, 1250);
		} else {
			ClocksActive(false);
			ResetTiles();
			model.ResetTiles();
			ResetLetterNumber();
			SetCurrentLevel();
			keyboardActive = true;

			CheckOk();
		}
	}

	void ResetLetterNumber() {
		letter.text = "";
		number.text = "";
	}

	public void OnNumberClick(){
		PlayDropSound ();
		number.text = "";
		CheckOk ();
	}

	public void OnLetterClick(){
		PlayDropSound ();
		letter.text = "";
		CheckOk ();
	}

	void ClocksActive(bool active) {
		clocks.ForEach((c) => c.gameObject.SetActive(active));
	}

	public void OkClick() {
		okBtn.interactable = false;
		keyboardActive = false;
		PlayWhackSound ();
		Invoke ("CheckWhack",0.5f);

	}

	private void CheckWhack(){
		int row = model.GetRow(number.text);
		int column = model.GetColumn(letter.text);

		if(model.HasTime()){
			
			TimeOkClick(row, column);
			SetLives(model.GetLives());
	

		} else {
			NoTimeOkClick(row, column);
		}
	}

	void SetLives(int livesModel) {
		for(int i = 0; i < lives.Count; i++) {
			lives[i].GetComponent<Button>().interactable = (livesModel > i);
		}
	}

	void ResetTiles() {
		foreach(var tile in tiles) {
			tile.sprite = tileSprites[GRASS_SPRITE];
		}
	}

	void NoTimeOkClick(int row, int column) {
		int slot = model.GetSlot(row, column);
		CheckSmackGrass(slot);
		if(IsCorrectNoTime(slot)) {
			//correct
			model.Correct();
			model.SmackMole();
			SmackMole(slot);

			Invoke ("CheckEndLevel", 1);
		} else {
			PlayWrongSound();
			keyboardActive = true;
		}
	}

	void CheckSmackGrass(int slot) {
		if(tiles[slot].sprite == tileSprites[GRASS_SPRITE]) {
			//TODO mary woosh sound!
			tiles[slot].sprite = tileSprites[GRASS_HOLE_SPRITE];
		}
	}

	bool IsCorrectNoTime(int slot) {
		return tiles[slot].sprite != tileSprites[GRASS_SPRITE] && tiles[slot].sprite != tileSprites[GRASS_HOLE_SPRITE] && tiles[slot].sprite != tileSprites[SMACKED_MOLE_SPRITE];
	}

	void TimeOkClick(int row, int column) {
		int slot = model.GetSlot(row, column);
		CheckSmackGrass(slot);
		if (model.IsCorrectTime(row, column)){
			SmackMole(slot);
			model.SmackMole();

			Invoke ("CheckEndLevel", 1);
		} else {
			PlayWrongSound();
			model.OneLessLife();

			SetLives(model.GetLives());

			if(model.NoMoreLives()){
				timerActive = false;
				EndGame(60, 0, 1250);
			} else keyboardActive = true;
		}
	}

	void SmackMole(int slot) {
		PlayWhackedMoleSound ();
		tiles[slot].sprite = tileSprites[SMACKED_MOLE_SPRITE];
		clocks[slot].gameObject.SetActive(false);

		if(model.HasTime()){
			topoCounter.text = (int.Parse(topoCounter.text) + 1).ToString();
		}

		ResetLetterNumber();
		CheckOk();
	}

	void CheckEndLevel() {
		if(model.IsLevelEnded()) {
			if(timerActive)
				timerActive = false;
			keyboardActive = false;
			ShowRightAnswerAnimation();
			model.NextLvl();
		} else
			keyboardActive = true;
	}

	public void Start(){
		model = new PlagasActivityModel();
		tileSprites = Resources.LoadAll<Sprite>("Sprites/PlagasActivity/tiles");
		whackSound = Resources.Load<AudioClip> ("Audio/PlagasActivity/whack");
		whackMoleSound = Resources.Load<AudioClip> ("Audio/PlagasActivity/whackedMole");
		topoCounter.text = "0";
		timerActive = false;
		switchTime = true;
		placaTopos.SetActive (false);
		Begin();
	}

	private void PlayWhackSound(){
		SoundController.GetController ().PlayClip (whackSound);
	}


	private void PlayWhackedMoleSound(){
		SoundController.GetController ().PlayClip (whackMoleSound);
	}

	public void Begin(){
		ShowExplanation();
	}

	private void SetCurrentLevel() {
		
		if(model.HasTime()){
			if (switchTime) {
				switchTime = false;
				Invoke("ShowNextLevelAnimation",1);
			} else {
				topoCounter.gameObject.SetActive(true);
				lives.ForEach ((GameObject g) => g.SetActive (true));
				TimeLevel(model.CurrentLvl());
				SetLives(model.GetLives());
			}

		} else {
			topoCounter.gameObject.SetActive(false);
			NormalLevel(model.CurrentLvl());
		}
	}

	void TimeLevel(PlagasLevel lvl) {
		TimerTiles(lvl);

		List<PlagaTile> modelTiles = model.GetTiles();

		//Set two starting veggies.
		for(int i = 0; i < PlagasActivityModel.VEGETABLES_IN_START; i++) {
			int slot = model.GetFreeSlot();

			tiles[slot].sprite = tileSprites[veggieRandomizer.Next()];
			modelTiles[slot].AppearInitVeggie();
		}

		StartTimer(true);
	}

	void TimerTiles(PlagasLevel lvl) {
		int randomSpawn = lvl.RandomSpawnTime();
		int moleQuantity = lvl.MolesInSpawn(randomSpawn);
		for(int i = 0; i < moleQuantity; i++) {
			int freeSlot = model.GetFreeSlot();
			model.SetTimerTile(freeSlot, randomSpawn);
		}
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

	void UpdateView() {
		model.DecreaseTimer();
		List<PlagaTile> modelTiles = model.GetTiles();
		bool newTimerSet = false;

		for(int i = 0; i < modelTiles.Count; i++) {
			PlagaTile tile = modelTiles[i];
			clocks[i].GetComponentInChildren<Text>(true).text = tile.GetTimer().ToString();

			//si tiene que aparecer un vegetal.
			if(tile.HasToAppear()){
				tiles[i].sprite = tileSprites[veggieRandomizer.Next()];
				modelTiles[i].AppearVeggie();

				if(!newTimerSet){
					newTimerSet = true;

					TimerTiles(model.CurrentLvl());
				}
			}

			//si sale vegetal y aparece topo.
			if(tile.MoleHasToAppear()){
				tiles[i].sprite = GetMoleFromVeggie(tiles[i].sprite);
				modelTiles[i].AppearMole();
				clocks[i].gameObject.SetActive(true);
				clocks[i].GetComponentInChildren<Text>(true).text = tile.GetTimer().ToString();
			}

			//si se termino el tiempo y no le pego.
			if(tile.TimeDoneAndNotSmacked()){
				tiles[i].sprite = tileSprites[GRASS_SPRITE];
				clocks[i].gameObject.SetActive(false);
				modelTiles[i].DissapearMole();

				if(model.HasTime()) {
					model.OneLessLife();
					SoundController.GetController ().PlayFailureSound ();
					SetLives(model.GetLives());
				}
			}

			//pasaron 5 segundos desde que esta mareado, se fue para abajo y dejo un pozo
			if(tile.IsSmackTimeDone()){
				tiles[i].sprite = tileSprites[MOLE_HOLE_SPRITE];
				modelTiles[i].SmackToHole();
			}

			if(model.NoMoreLives()){
				timerActive = false;
				EndGame(60, 0, 1250);
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
		PlaySoundClick ();
		letter.text = l;
		CheckOk();
	}

	public void NumberClick(string n){
		PlaySoundClick ();
		number.text = n;
		CheckOk();
	}

	void CheckOk() {
		okBtn.interactable = CanSubmit();
	}

	bool CanSubmit() {
		return letter.text.Length == 1 && number.text.Length == 1;
	}

	override public void RestartGame(){
		base.RestartGame ();
		Start();
	}

	void OnGUI() {
		Event e = Event.current;
		if (e.isKey && e.type == EventType.KeyUp && keyboardActive) {
			if (e.keyCode >= KeyCode.A && e.keyCode <= KeyCode.F) {
				Debug.Log ("Detected key code: " + e.keyCode);
				LetterClick(e.keyCode.ToString());
			} else if (e.keyCode == KeyCode.Return) {
				Debug.Log ("Detected key code: Enter");
				if(okBtn.interactable) OkClick();
			} else if((e.keyCode >= KeyCode.Alpha1 && e.keyCode <= KeyCode.Alpha6) || (e.keyCode >= KeyCode.Keypad1 && e.keyCode <= KeyCode.Keypad6)) {
				NumberClick(e.keyCode.ToString()[e.keyCode.ToString().Length - 1].ToString());
			}
		}
	}

	override public void OnNextLevelAnimationEnd(){
		EnableComponents (true);
		topoCounter.gameObject.SetActive(true);
		lives.ForEach ((GameObject g) => g.SetActive (true));
		TimeLevel(model.CurrentLvl());
		SetLives(model.GetLives());
		placaTopos.SetActive (true);
		PlayTimeLevelMusic ();
	}

}