using Assets.Scripts.Games;
using System.Collections.Generic;
using UnityEngine;

public abstract class RaulLevel  {


    protected RaulStage currentStage;


    protected Sprite[] animalSpriteEnunciados;

    protected Sprite[] animalSpriteResultados;

    protected List<int> randomListGenenrator;



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

    public abstract void GetNewOption();

    public void SetNewStage(RaulStage newStage)
    {
        currentStage = newStage;
        currentStage.SetView();
    }

}
