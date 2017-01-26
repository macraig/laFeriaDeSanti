using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Games;
using UnityEngine;
using Assets.Scripts.Metrics.Model;
using SimpleJSON;
using Assets.Scripts.Common;

public class PlagasActivityModel : LevelModel {
	private const int TILES = 36;
	//time is in seconds
	private const int MOLE_TIME = 9, VEGETABLE_TO_MOLE = 2, VEGETABLES_IN_START = 2, MOLES_TO_NEXT_LEVEL = 5;
	private List<PlagaTile> tiles;
	private int smackedMoles;

	private int currentLvl, exercisesDone;
	List<PlagasLevel> lvls;

	public PlagasActivityModel() {
		exercisesDone = 0;
		currentLvl = 0;
		smackedMoles = 0;
		StartLevels();
		ResetTiles();
		MetricsController.GetController().GameStart();
	}

	public int GetFreeSlot() {
		Randomizer tileRandomizer = Randomizer.New(tiles.Count - 1);
		bool valid = false;

		int nextSpot = -1;
		while(!valid){
			nextSpot = tileRandomizer.Next();
			if(IsSpotFree(nextSpot)) valid = true;
		}
		return nextSpot;
	}

	public bool IsSpotFree(int spot) {
		return tiles[spot].GetState() == PlagaState.FREE;
	}

	public void DecreaseTimer() {
		foreach(PlagaTile tile in tiles) {
			tile.DecreaseTimer();
		}
	}

	public void SetTimerTile(int freeSlot, int randomSpawn) {
		tiles[freeSlot].SetTimeToAppear(randomSpawn);
	}

	void ResetTiles() {
		tiles = new List<PlagaTile>();
		for(int i = 0; i < TILES; i++) {
			tiles.Add(new PlagaTile());
		}
	}

	void StartLevels() {
		lvls = new List<PlagasLevel>();
		JSONArray lvlsJson = JSON.Parse(Resources.Load<TextAsset>("Jsons/PlagasActivity/levels").text).AsObject["levels"].AsArray;
		foreach(JSONNode lvlJson in lvlsJson) {
			lvls.Add(new PlagasLevel(lvlJson.AsObject));
		}
	}

	public List<PlagaTile> GetTiles() {
		return tiles;
	}

	public PlagasLevel CurrentLvl() {
		return lvls[currentLvl];
	}

	public bool HasTime() {
		return CurrentLvl().HasTime();
	}

	public bool GameEnded(){
		return exercisesDone == 6;
	}

	public void NextLvl(){
		currentLvl++;
	}

	public void Correct() {
		LogAnswer(true);

		smackedMoles++;
	}

	void AddExerciseDone() {
		exercisesDone++;
	}

	public void Wrong(){
		LogAnswer(false);
	}

	public bool IsCorrect(string letter, string number) {
		return true;
	}
}
