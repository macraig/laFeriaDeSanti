using Assets.Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaulButton : MonoBehaviour {

    public bool isCorrect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowAnswer()
    {
        if (isCorrect)
        {
            RaulSaysController.instance.CorrectOptionChosen();
        }
        else
        {
            RaulSaysController.instance.IncorrectOptionChosen();
        }
    }
}
