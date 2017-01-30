using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RaulStage  {


    public List<GameObject> enunciados;
    public List<AudioClip> enunciadosAudios;
    

    public abstract void ShowNextEnunciado(int randomResult, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado);

    public abstract void SetView();

    public abstract void UpdateLevelValues(int newLevel);
}
