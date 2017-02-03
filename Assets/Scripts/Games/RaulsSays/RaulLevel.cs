using Assets.Scripts.Games;
using System.Collections.Generic;
using UnityEngine;

public class RaulLevel  {


    private RaulStage currentStage;


    private Sprite[] animalSpriteEnunciados;

    private Sprite[] animalSpriteResultados;

    private List<int> randomListGenenrator;

    private bool viewSetted;
    private int optionLength;
    private int randomLength;

    public RaulStage CurrentStage
    {
        get
        {
            return currentStage;
        }

        

    }

    public RaulLevel(Sprite[] animalSprite)
    {
        

        randomListGenenrator = new List<int>();
        for(int i = 0; i < animalSprite.Length/2; i++)
        {
            randomListGenenrator.Add(i);
        }

        animalSpriteEnunciados = new Sprite[animalSprite.Length / 2];
        animalSpriteResultados = new Sprite[animalSprite.Length / 2];

        for (int i = 0; i < animalSprite.Length / 2; i++)
        {
            animalSpriteEnunciados[i] = animalSprite[i];
            animalSpriteResultados[i] = animalSprite[(animalSprite.Length / 2) + i];
        }

        viewSetted = false;

    }

    public void GetNewOption()
    {

        if (!viewSetted)
        {
            if (optionLength == 4)
            {
                RaulSaysController.instance.view.SetLevel1();
            }else if(optionLength == 8 && randomLength==4)
            {
                RaulSaysController.instance.view.SetLevel2();
            }else
            {
                RaulSaysController.instance.view.SetLevel3();
            }
            viewSetted = true;
        }

        ShuffleList();

        Sprite[] restAnimalEnunciado = new Sprite[optionLength];
        Sprite[] restAnimalResultado = new Sprite[optionLength];

        for (int i = 0; i < restAnimalEnunciado.Length; i++)
        {
            restAnimalEnunciado[i] = animalSpriteEnunciados[randomListGenenrator[i]];
            restAnimalResultado[i] = animalSpriteResultados[randomListGenenrator[i]];
        }

        int randomResult = Random.Range(0, randomLength);

        currentStage.ShowNextEnunciado(randomResult, restAnimalEnunciado, restAnimalResultado);
    }

    public void ShuffleList()
    {

        System.Random _random = new System.Random();

        int myGO;

        int n = randomListGenenrator.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = randomListGenenrator[r];
            randomListGenenrator[r] = randomListGenenrator[i];
            randomListGenenrator[i] = myGO;
        }

        
    }


    public void SetNewStage(RaulStage newStage)
    {
        currentStage = newStage;
        currentStage.SetView();
    }

    public void SetNewLevelValue(int levelValue)
    {
        viewSetted = false;

        if (levelValue == 0)
        {
            optionLength = 4;
            randomLength = 4;
        }else if(levelValue == 1)
        {
            optionLength = 8;
            randomLength = 4;
        }
        else if(levelValue == 2)
        {
            optionLength = 8;
            randomLength = 8;
        }
    }

}
