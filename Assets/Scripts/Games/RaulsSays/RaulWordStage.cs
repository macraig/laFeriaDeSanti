using Assets.Scripts.Games;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaulWordStage : RaulStage
{

    private string[][] indicators;

    public RaulWordStage()
    {
        indicators = new string[4][];


        


    }


    public string[] GetNextEnunciado(int random)
    {
        return indicators[random];
    }

    public override void ShowNextEnunciado(int randomResult, Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
    {
        string[] stringToShow = GetNextEnunciado(randomResult);

        RaulSaysController.instance.view.ShowWordOption(randomResult, stringToShow, restAnimalEnunciado, restAnimalResultado);
    }

    public override void SetView()
    {
        RaulSaysController.instance.view.SetWordStage();
    }

    public override void UpdateLevelValues(int currentLevel)
    {
        if (currentLevel == 0)
        {

            indicators[0] = new string[1];
            indicators[0][0] = "ABAJO";

            indicators[1] = new string[1];
            indicators[1][0] = "ARRIBA";

            indicators[2] = new string[1];
            indicators[2][0] = "IZQUIERDA";

            indicators[3] = new string[1];
            indicators[3][0] = "DERECHA";

        }

        else if(currentLevel == 1)
        {
            indicators[0] = new string[2];
            indicators[0][0] = "ABAJO";
            indicators[0][1] = "IZQUIERDA";

            indicators[1] = new string[2];
            indicators[1][0] = "ABAJO";
            indicators[1][1] = "DERECHA";

            indicators[2] = new string[2];
            indicators[2][0] = "ARRIBA";
            indicators[2][1] = "IZQUIERDA";

            indicators[3] = new string[2];
            indicators[3][0] = "ARRIBA";
            indicators[3][1] = "DERECHA";


        }
        else
        {
            indicators = new string[8][];


            for (int i = 0; i < 8; i++)
            {

                if (i < 4)
                {
                    indicators[i] = new string[1];
                    
                }
                else
                {
                    indicators[i] = new string[2];
                }

            }

            indicators[0][0] = "ABAJO";
            indicators[1][0] = "ARRIBA";
            indicators[2][0] = "IZQUIERDA";
            indicators[3][0] = "DERECHA";

            indicators[4][0] = "ABAJO";
            indicators[4][1] = "IZQUIERDA";

            indicators[5][0] = "ABAJO";
            indicators[5][1] = "DERECHA";

            indicators[6][0] = "ARRIBA";
            indicators[6][1] = "IZQUIERDA";

            indicators[7][0] = "ARRIBA";
            indicators[7][1] = "DERECHA";

        }
    }
}