using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Games;

public class RaulLevel2 : RaulLevel
{



    public RaulLevel2(Sprite[] animalSprite) : base(animalSprite)
    {
        
        RaulSaysController.instance.view.SetLevel2();

    }

    public override void GetNewOption()
    {

        ShuffleList();

        Sprite[] restAnimalEnunciado = new Sprite[8];
        Sprite[] restAnimalResultado = new Sprite[8];

        for (int i = 0; i < 8; i++)
        {
            restAnimalEnunciado[i] = animalSpriteEnunciados[randomListGenenrator[i]];
            restAnimalResultado[i] = animalSpriteResultados[randomListGenenrator[i]];
        }

        int randomResult = Random.Range(0, 4);

        currentStage.ShowNextEnunciado(randomResult, restAnimalEnunciado, restAnimalResultado);
    }

  
}

