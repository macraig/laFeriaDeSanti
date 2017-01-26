using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Games;
using UnityEngine;
using Assets.Scripts.Metrics.Model;

public class PlagasActivityModel : LevelModel {
	private int currentLvl, exercisesDone;

	public bool GameEnded(){
		return exercisesDone == 6;
	}

	public bool IsTimeLevel(){
		return exercisesDone >= 3;
	}

	public int CurrentLvl(){
		return currentLvl;
	}

	public void NextLvl(){
		currentLvl++;
	}

	public PlagasActivityModel() {
		exercisesDone = 0;
		currentLvl = 0;
		MetricsController.GetController().GameStart();

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
