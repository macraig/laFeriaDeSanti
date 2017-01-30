using Assets.Scripts.Games;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaulArrowStage : RaulStage
{

    private Sprite[][] arrowsToShow;
    private Sprite[] arrows;

    public RaulArrowStage(Sprite[] arrows)
    {
        arrowsToShow = new Sprite[4][];
        this.arrows = arrows;


        
    }


    public Sprite[] GetNextEnunciado(int random)
    {
        return arrowsToShow[random];
    }

    public override void ShowNextEnunciado(int randomResult, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
    {
        Sprite[] spriteToShow = GetNextEnunciado(randomResult);

        RaulSaysController.instance.view.ShowArrowOption(randomResult, spriteToShow, restAnimalEnunciado, restAnimalResultado);
    }

    public override void SetView()
    {
        RaulSaysController.instance.view.SetArrowStage();
    }

    public override void UpdateLevelValues(int currentLevel)
    {
        if (currentLevel == 0)
        {

            for (int i = 0; i < arrows.Length; i++)
            {
                arrowsToShow[i] = new Sprite[1];
                arrowsToShow[i][0] = arrows[i];
            }
        }

        else if(currentLevel == 1)
        {

            for (int i = 0; i < arrows.Length; i++)
            {
                arrowsToShow[i] = new Sprite[2];
            }

            arrowsToShow[0][0] = arrows[0];
            arrowsToShow[0][1] = arrows[2];

            arrowsToShow[1][0] = arrows[0];
            arrowsToShow[1][1] = arrows[3];

            arrowsToShow[2][0] = arrows[1];
            arrowsToShow[2][1] = arrows[2];

            arrowsToShow[3][0] = arrows[1];
            arrowsToShow[3][1] = arrows[3];
        }
        else
        {
            arrowsToShow = new Sprite[8][];


            for (int i = 0; i < 8; i++)
            {

                if (i < 4)
                {
                    arrowsToShow[i] = new Sprite[1];
                    arrowsToShow[i][0] = arrows[i];
                }else
                {
                    arrowsToShow[i] = new Sprite[2];
                }

            }

            arrowsToShow[4][0] = arrows[0];
            arrowsToShow[4][1] = arrows[2];

            arrowsToShow[5][0] = arrows[0];
            arrowsToShow[5][1] = arrows[3];

            arrowsToShow[6][0] = arrows[1];
            arrowsToShow[6][1] = arrows[2];

            arrowsToShow[7][0] = arrows[1];
            arrowsToShow[7][1] = arrows[3];




        }
    }
}
