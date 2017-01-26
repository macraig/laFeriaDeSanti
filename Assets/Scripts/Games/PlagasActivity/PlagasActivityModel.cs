using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Games;
using UnityEngine;
using Assets.Scripts.Metrics.Model;
using SimpleJSON;

public class PlagasActivityModel : LevelModel {
	//time is in seconds
	private const int MOLE_TIME = 9, VEGETABLE_TO_MOLE = 2, VEGETABLES_IN_START = 2, MOLES_TO_NEXT_LEVEL = 5;

	private int currentLvl, exercisesDone;
	List<PlagasLevel> lvls;

	public PlagasActivityModel() {
		exercisesDone = 0;
		currentLvl = 0;
		StartLevels();
		MetricsController.GetController().GameStart();
	}

	void StartLevels() {
		lvls = new List<PlagasLevel>();
		JSONArray lvlsJson = JSON.Parse(Resources.Load<TextAsset>("Jsons/PlagasActivity").text).AsObject["levels"].AsArray;
		foreach(JSONNode lvlJson in lvlsJson) {
			lvls.Add(new PlagasLevel(lvlJson.AsObject));
		}
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

		AddExerciseDone();
	}

	void AddExerciseDone() {
		exercisesDone++;
	}

	public void Wrong(){
		LogAnswer(false);
	}
}
