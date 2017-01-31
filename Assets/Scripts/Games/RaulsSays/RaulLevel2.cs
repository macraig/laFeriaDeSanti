using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Games;

public class RaulLevel2 : RaulLevel
{

    private bool viewSetted;

    public RaulLevel2(Sprite[] animalSprite) : base(animalSprite)
    {
        viewSetted = false;

    }

    public override void GetNewOption()
    {

        if (!viewSetted)
        {
            RaulSaysController.instance.view.SetLevel2();

        }

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

