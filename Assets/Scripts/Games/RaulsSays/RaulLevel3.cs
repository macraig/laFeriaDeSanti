using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Games;

namespace Assets.Scripts.Games
{
    internal class RaulLevel3 : RaulLevel
    {



        public RaulLevel3(Sprite[] animalSprite) : base(animalSprite)
        {
            RaulSaysController.instance.view.SetLevel3();
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

            int randomResult = Random.Range(0, 8);

            currentStage.ShowNextEnunciado(randomResult, restAnimalEnunciado, restAnimalResultado);
        }


       


    }
}