using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Games;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;
using System.Security.Policy;

public class PlagasActivityView : LevelView {
	public List<Image> pattern;
	public Button soundBtn, okBtn, NextButton;
	public List<Button> droppers;
	public List<Image> tiles, clocks;
	public Text time;

	private Sprite[] tileSprites;
	private PlagasActivityModel model;
	public AudioClip ingameExplanationSound;

	override public void Next(bool first = false){

		if (!first) PlaySoundClick();
		okBtn.enabled = true;
		NextButton.gameObject.SetActive(false);
		okBtn.gameObject.SetActive(true);
		okBtn.enabled = true;
		SetCurrentLevel();
	}

	public void OkClick()
	{
		okBtn.enabled = false;
		if (IsCorrect()){
			model.Correct();
			ShowRightAnswerAnimation();
		} else {
			ShowWrongAnswerAnimation();
			model.Wrong();
		}
	}

	public void OnSoundButtonClick(){
		SoundController.GetController ().PlayClip (ingameExplanationSound);
	}

	bool IsCorrect() {
		for(int i = 0; i < pattern.Count; i++) {
			if(droppers[i].image.sprite != pattern[i].sprite) return false;
			if(droppers[i + 4].image.sprite != pattern[i].sprite) return false;
		}

		return true;
	}

	public void EraseDropper(Button dropper){
		dropper.image.sprite = null;
		dropper.image.color = new Color32(0,0,0,1);

		okBtn.interactable = false;
		SoundController.GetController().PlayClickSound();
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
	}

	void CleanDroppers() {
		droppers.ForEach(EraseDropper);
	}

	public void Dropped() { CheckOk(); }

	void CheckOk() {
		okBtn.interactable = CanSubmit();
	}

	bool CanSubmit() {
		foreach(Button dropper in droppers) {
			if(dropper.image.sprite == null) return false;
		}
		return true;
	}
}
