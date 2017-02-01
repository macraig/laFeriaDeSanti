using Assets.Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    private RecorridosController.TileEnum type;

    private Vector3 position;
    private Sprite sprite;



    public Tile(RecorridosController.TileEnum type, Vector3 currentPosition, Sprite sprite)
    {
        this.type = type;
        this.position = currentPosition;
        this.sprite = sprite;
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }


    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }
       
}
