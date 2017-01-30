using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Games;

public class RaulLevel1 : RaulLevel {



    public RaulLevel1(Sprite[] animalSprite):base(animalSprite)
    {
        RaulSaysController.instance.view.SetLevel1();
    }

    public override void GetNewOption()
    {

        ShuffleList();

        Sprite[] restAnimalEnunciado = new Sprite[4];
        Sprite[] restAnimalResultado =  new Sprite[4];

        for (int i = 0; i < 4; i++)
        {
            restAnimalEnunciado[i] = animalSpriteEnunciados[randomListGenenrator[i]];
            restAnimalResultado[i] = animalSpriteResultados[randomListGenenrator[i]];
        }

        int randomResult = Random.Range(0, 4);
        
        currentStage.ShowNextEnunciado(randomResult, restAnimalEnunciado, restAnimalResultado);
    }


}
