﻿using Assets.Scripts.Games.Shipments;
using UnityEngine;
using UnityEngine.UI;

public class MapPlace : MonoBehaviour
{
    public Image CrossReference;
    private int _id;
    private bool _isIntermediate;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    void Start()
    {
        Button.ButtonClickedEvent click = GetComponent<Button>().onClick;
         click.AddListener(OnClickMapPlace);
    }

    public void SetData(int id, Sprite placesSprite, Sprite crossSprite, bool isIntermediate)
    {
        this.Id = id;
        CrossReference.sprite = crossSprite;
        GetComponent<Image>().sprite = placesSprite;
        _isIntermediate = isIntermediate;
    }

    private void OnClickMapPlace()
    {
        ShipmentsView.instance.OnClickMapPlace(_id, _isIntermediate);
    }
}