using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games
{
    public class RecorridosController : MonoBehaviour
    {

        public static RecorridosController instance;

        public enum TileEnum { Path, Hole, Wall, Start, End, Nut, Bomb, Fire }

        public GameObject boardPosition;
        public Image player;

        public Sprite pathSprite;



        public Sprite holeSprite;
        public Sprite wallSprite;
        public Sprite startSprite;
        public Sprite endSprite;
        public Sprite nutSprite;
        public Sprite bombSprite;
        public Sprite fireSprite;

        private Tile[][] gridSpace;

        private List<RecorridosButton> actionsToDo;

        private RecorridosView view;
        private Vector2 puppetPosition;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }else
            {
                Destroy(gameObject);
            }
        }


        // Use this for initialization
        void Start()
        {
            gridSpace = new Tile[7][];
            BuildRandomBoard();
            player.transform.position = gridSpace[0][0].Position;
            actionsToDo = new List<RecorridosButton>();
            view = GetComponent<RecorridosView>();
            puppetPosition = new Vector2(0, 0);
        }

        private void BuildRandomBoard()
        {
            int rowCounter = 0;
            gridSpace[rowCounter] = new Tile[7];

            for (int i = 0; i < boardPosition.transform.childCount; i++)
            {
                if (i % 7 == 0 && i!=0)
                {
                    rowCounter++;
                    gridSpace[rowCounter] = new Tile[7];
                }
                if (rowCounter==0 && i == 0)
                {
                    gridSpace[rowCounter][i % 7] = new Tile(TileEnum.Start,
                        boardPosition.transform.GetChild(i).transform.position, startSprite);
                }
                else if(rowCounter==6 && i == boardPosition.transform.childCount-1)
                {
                    gridSpace[rowCounter][i % 7] = new Tile(TileEnum.End,
                        boardPosition.transform.GetChild(i).transform.position, endSprite);
                }
                else
                {
                    gridSpace[rowCounter][i % 7] = new Tile(TileEnum.Path,
                        boardPosition.transform.GetChild(0).transform.position, pathSprite);
                }
                boardPosition.transform.GetChild(i).GetComponent<Image>().sprite = gridSpace[rowCounter][i % 7].Sprite;

            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddAction(RecorridosButton recorridosButton)
        {
            switch (recorridosButton.currentAction)
            {
                case RecorridosButton.ActionToDo.Start:
                    
                    MovePuppet();
                    break;
                case RecorridosButton.ActionToDo.Remove:
                    actionsToDo.RemoveAt(recorridosButton.indexInList);
                    view.RemoveInstruction(recorridosButton.indexInList);
                    break;
                default:
                    if (actionsToDo.Count < 5)
                    {
                        actionsToDo.Add(recorridosButton);
                        view.AddInstruction(recorridosButton);
                    }
                break;
            }



        }

        public void MovePuppet()
        {
            bool isActionDoable ;
            foreach(RecorridosButton action in actionsToDo)
            {
                switch (action.currentAction)
                {
                    case RecorridosButton.ActionToDo.Up:
                        //if((puppetPosition.x + 1) >= 0)
                        break;
                    case RecorridosButton.ActionToDo.Down:

                        break;
                    case RecorridosButton.ActionToDo.Left:
                        break;
                    case RecorridosButton.ActionToDo.Right:
                        break;
                    default:
                        break;
                }
            }
        }



    }
}
