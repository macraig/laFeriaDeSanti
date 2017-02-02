using System;
using UnityEngine.EventSystems;
using UnityEngine;
using Assets.Scripts.Games.TiteresActivity;
using Assets.Scripts.Sound;
using UnityEngine.UI;

public class TiteresSlot : MonoBehaviour, IDropHandler {
	public TiteresActivityView view;

	public void OnDrop(PointerEventData eventData) {
		TiteresDragger target = TiteresDragger.itemBeingDragged;
		if(target != null) {
			SoundController.GetController().PlayDropSound();
			target.DroppedInLandscape();
			view.CheckOk();
		}
	}
}