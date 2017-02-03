using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Metrics.Model;
using Assets.Scripts.Common;
using SimpleJSON;
using Assets.Scripts.Games;
using Assets.Scripts.Metrics;

public class TiteresActivityModel : LevelModel {
	//time is in seconds
	public const int START_TIME = 60, CORRECT_SCENE_TIME = 15;
	public static List<string> NAMES = new List<string>{ "INÉS", "PEDRO", "ARTURO", "LUCÍA" };
	public static List<string> OBJECT_NAMES = new List<string>{ "de la palmera", "de la vaca", "del hongo", "del cactus","del pingüino",
		"del zorro","del tronco","del cactus","del tractor","de los huesos"};
	private int timer;
	private bool withTime;

	private int currentLvl;
	List<TiteresLevel> lvls;

	public TiteresActivityModel() {
		currentLvl = 0;
		timer = START_TIME;
		withTime = false;
		StartLevels();
		MetricsController.GetController().GameStart();
	}

	public bool GameEnded(){
		return currentLvl == lvls.Count;
	}



	void StartLevels(bool withTime = false) {
		lvls = new List<TiteresLevel>();
		JSONArray lvlsJson = JSON.Parse(Resources.Load<TextAsset>("Jsons/TiteresActivity/levels").text).AsObject["levels"].AsArray;
		foreach(JSONNode lvlJson in lvlsJson) {
			lvls.Add(new TiteresLevel(lvlJson.AsObject, withTime));
		}
	}

	public TiteresLevel CurrentLvl() {
		return lvls[currentLvl];
	}

	public bool HasTime() {
		return withTime;
	}



	public void NextLvl(){
		currentLvl++;

		if(currentLvl == lvls.Count){
			withTime = true;
			currentLvl = 0;
			StartLevels(true);
		}
	}

	public void WithTime(){
		withTime = true;
	}

	public void Correct() {
		LogAnswer(true);
	}

	public void Wrong(){
		LogAnswer(false);
	}

	public void DecreaseTimer() {
		if(timer > 0) timer--;
	}

	public bool IsTimerDone(){
		return timer == 0;
	}

	public void CorrectTimer() {
		timer += CORRECT_SCENE_TIME;
	}

	public int GetTimer() {
		return timer;
	}
}
