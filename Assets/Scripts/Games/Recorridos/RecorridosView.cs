using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecorridosView : MonoBehaviour {

    public GameObject instructionsStack;

    private List<GameObject> stackImages;
    private int currentAvailableInstructionSpot;


    private void Start()
    {
        stackImages = new List<GameObject>();
        for(int i = 0; i < instructionsStack.transform.childCount; i++)
        {
            stackImages.Add(instructionsStack.transform.GetChild(i).gameObject);
            stackImages[i].SetActive(false);
            stackImages[i].GetComponent<RecorridosButton>().indexInList = i;
        }
        currentAvailableInstructionSpot = 0;
    }

    public void AddInstruction(RecorridosButton actionToAdd)
    {
        stackImages[currentAvailableInstructionSpot].GetComponent<Image>().sprite = actionToAdd.sprite;
        stackImages[currentAvailableInstructionSpot].SetActive(true);
        //stackImages[currentAvailableInstructionSpot].GetComponent<RecorridosButton>().indexInList = currentAvailableInstructionSpot;
        for(int i = 0; i < stackImages.Count; i++)
        {
            if (!stackImages[i].activeSelf)
            {
                currentAvailableInstructionSpot = i;
                return;
            }
        }
    }

    public void RemoveInstruction(int instructionIndex)
    {

        if (instructionIndex == stackImages.Count - 1)
        {
            currentAvailableInstructionSpot = instructionIndex;
            stackImages[instructionIndex].SetActive(false);
        }
        else if (!stackImages[instructionIndex + 1].activeSelf)
        {
            currentAvailableInstructionSpot = instructionIndex;
            stackImages[instructionIndex].SetActive(false);

        }
        else
        {

            while (stackImages[instructionIndex+1].activeSelf)
            {
                stackImages[instructionIndex].GetComponent<Image>().sprite = stackImages[instructionIndex + 1].GetComponent<Image>().sprite;
                instructionIndex++;
                if(instructionIndex == stackImages.Count-1)
                {
                    break;
                }
            }

            currentAvailableInstructionSpot = instructionIndex;
            stackImages[instructionIndex].SetActive(false);
        }




    }

}
