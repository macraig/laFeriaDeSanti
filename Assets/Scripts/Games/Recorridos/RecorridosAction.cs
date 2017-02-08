using Assets.Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Games.Recorridos
{
public class RecorridosAction : MonoBehaviour {

    public enum ActionToDo { Up, Down, Left, Right, Remove,Start}

    public ActionToDo currentAction;
    public Sprite sprite;

    public int indexInList;
 

    public void DoAction()
    {
        RecorridosController.instance.AddAction(this);
    }



}
}