using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Metrics.Model;
using Assets.Scripts.Common;
using SimpleJSON;
using Assets.Scripts.Games;

public class TiteresActivityModel : LevelModel {
	//time is in seconds
	public const int START_TIME = 60, CORRECT_SCENE_TIME = 15;
	public static List<string> NAMES = new List<string>{ "INÉS", "PEDRO", "ARTURO", "LUCÍA" };

	private int currentLvl;
	List<TiteresLevel> lvls;

	public TiteresActivityModel() {
		currentLvl = 0;
		StartLevels();
		MetricsController.GetController().GameStart();
	}

	public bool GameEnded(){
		return currentLvl == lvls.Count;
	}

	void StartLevels() {
		lvls = new List<TiteresLevel>();
		JSONArray lvlsJson = JSON.Parse(Resources.Load<TextAsset>("Jsons/TiteresActivity/levels").text).AsObject["levels"].AsArray;
		foreach(JSONNode lvlJson in lvlsJson) {
			lvls.Add(new TiteresLevel(lvlJson.AsObject));
		}
	}

	public TiteresLevel CurrentLvl() {
		return lvls[currentLvl];
	}

	public bool HasTime() {
		return CurrentLvl().HasTime();
	}

	public void NextLvl(){
		currentLvl++;
	}

	public void Correct() {
		LogAnswer(true);
	}

	public void Wrong(){
		LogAnswer(false);
	}
}
