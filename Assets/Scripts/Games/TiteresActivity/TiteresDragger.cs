using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;
using Assets.Scripts.Games.TiteresActivity;
using System.Collections.Generic;

public class TiteresDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static TiteresDragger itemBeingDragged;
	private Vector3 startPosition;
	public bool active;
	private bool droppedInLandscape;
	//Position where dragger starts in screen.
	private Vector3 initialPosition;
	private int starterSpriteIndex;
	public List<Sprite> puppets;

	public TiteresActivityView view;

	public void Start(){
		initialPosition = transform.position;
		droppedInLandscape = false;
		puppets = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/TiteresActivity/puppets"));
		starterSpriteIndex = puppets.IndexOf (GetComponent<Image> ().sprite);
	}

	public void SetActive(bool isActive){
		active = isActive;
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if (active) {
			transform.SetAsLastSibling ();
			SoundController.GetController ().SetConcatenatingAudios (false);
			view.soundBtn.interactable = true;
			SoundController.GetController().PlayDragSound();
			itemBeingDragged = this;
			startPosition = transform.position;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
			droppedInLandscape = false;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		if (active) 
			transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData = null) {
		if (active) {
			SoundController.GetController ().SetConcatenatingAudios (false);
			view.soundBtn.interactable = true;
			SoundController.GetController().PlayDropSound();
			itemBeingDragged = null;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;

			if(!droppedInLandscape){
				transform.position = startPosition;
			}
		}
	}

	public void DroppedInLandscape(){
		droppedInLandscape = true;
	}

	public void SetToInitialPosition() {
		droppedInLandscape = false;
		transform.position = initialPosition;

	}

	public bool IsDroppedInLandscape() {
		return droppedInLandscape;
	}

	public void ClickDragger(){
		Sprite currentSprite = GetComponent<Image>().sprite;

		int index = puppets.IndexOf(currentSprite);

		if(index % 4 == 3) index = index - 3;
		else index++;

		GetComponent<Image>().sprite = puppets[index];
	}

	public void ResetPuppetSprite(){
		GetComponent<Image>().sprite = puppets[starterSpriteIndex];
	}
}