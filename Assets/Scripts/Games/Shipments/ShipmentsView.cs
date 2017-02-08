using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games.Shipments
{
    public class ShipmentsView : LevelView
    {

        public static ShipmentsView instance; 

        public MapGenerator MapGenerator;
        public GameObject[] AnswerRowGameObjects;
        public Sprite[] AnswerCellSprites;
        public Button OkButton;
        public Button NextButton;
        public Color FocusedColor;
        public Color UnfocusedColor;
        public Text ScaleText;
        private int _currentFocus;
        public Image StartPlace;
        public Image FinishPlace;

        public GameObject Player;
        public Image[] PlayeSprites;

        public ShipmentsModel Model { get; set; }

        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);
        }
        
        // Use this for initialization
        void Start ()
        {
            AddCellLiseners();
            GetAnswerCells()[0].Value = 0;
            Model = new ShipmentsModel();
            
            HighlightCurrentFocus();
            menuBtn.onClick.AddListener(OnClickMenuBtn);
            Next(true);
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { ChangeFocusCell(_currentFocus + 1); }        
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { ChangeFocusCell(_currentFocus - 1); }        
            if (Input.GetKeyDown(KeyCode.UpArrow)) { ChangeFocusCell(_currentFocus - 3); }        
            if (Input.GetKeyDown(KeyCode.DownArrow)) { ChangeFocusCell(_currentFocus + 3); }
            else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) { OnClickNumberBtn(0); }
            else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) { OnClickNumberBtn(1); }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) { OnClickNumberBtn(2); }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) { OnClickNumberBtn(3); }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) { OnClickNumberBtn(4); }
            else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) { OnClickNumberBtn(5); }
            else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) { OnClickNumberBtn(6); }
            else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) { OnClickNumberBtn(7); }
            else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) { OnClickNumberBtn(8); }
            else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) { OnClickNumberBtn(9); }
            else if (Input.GetKeyDown(KeyCode.Backspace)) { OnClickClear(); }
        }

        

        public void OnClickNumberBtn(int number)
        {
            ShipmentsAnswerCell cell = GetCurrentAnswerCell();
            if (cell.Type == AnswerCellType.Numeric)
            {
                Text uiText = cell.GetComponentInChildren<Text>();
                if (uiText.text.Length > 2)
                {
                    SoundController.GetController().PlayDropSound();
                }
                else
                {
                    if (cell.Value == -1)
                    {
                        uiText.text = "";
                    }
                    SoundController.GetController().PlayTypingSound();
                    uiText.text += number;
                    cell.Value = int.Parse(uiText.text);
                }
            }
            
        }

        public void OnClickClear()
        {
            SoundController.GetController().PlayTypingSound();
            GetCurrentAnswerCell().Clear();
        }

        private ShipmentsAnswerCell GetCurrentAnswerCell()
        {
            return GetAnswerCells()[_currentFocus];
        }

        public void ChangeFocusCell(int cell)
        {
            if(cell < 1 || cell > GetAnswerCells().Count - 1) return;
            PlaySoundClick();
            UnhighlightCurrentFocus();
            _currentFocus = cell;
            HighlightCurrentFocus();
        }

        private void HighlightCurrentFocus()
        {
            GetCurrentAnswerCell().GetComponent<Button>().image.color = FocusedColor;
        }

        private void UnhighlightCurrentFocus()
        {
            GetCurrentAnswerCell().Unpaint();
        }

        public override void Next(bool first = false)
        {
            if(!first) PlaySoundClick();
            OkButton.gameObject.SetActive(true);
            OkButton.enabled = true;
            NextButton.gameObject.SetActive(false);
            Model.NextExercise();
            MapGenerator.SafeLocatePlaces(Model.Nodes, Model.Edges);
                     
            MapGenerator.TraceEdges(Model.Edges);
            ScaleText.text = Model.Scale + " km";
            ClearAnswers();
            StartPlace.sprite =
                MapGenerator.Places.Find(e => e.Type == ShipmentNodeType.Start).GetComponent<Image>().sprite;
            FinishPlace.sprite =
                MapGenerator.Places.Find(e => e.Type == ShipmentNodeType.Finish).GetComponent<Image>().sprite;
            _currentFocus = 1;
            ChangeFocusCell(1);
            SetPlayerToPlace(0);
        }

        private void SetPlayerToPlace(int i)
        {
            MapPlace place = MapGenerator.Places.Find(e => e.Id == i);
            Player.transform.SetParent(place.transform);
            Player.transform.localPosition = Vector2.zero;
        }

        private void ClearAnswers()
        {
            foreach (ShipmentsAnswerCell shipmentsAnswerCell in GetAnswerCells())
            {
                if(shipmentsAnswerCell.Value == 0) continue;
                shipmentsAnswerCell.Clear();
            }
        }


        private List<ShipmentsAnswerCell> GetAnswerCells()
        {
            List<ShipmentsAnswerCell> answerCells = new List<ShipmentsAnswerCell>();
            foreach (GameObject answerRowGameObject in AnswerRowGameObjects)
            {
                answerCells.AddRange(answerRowGameObject.GetComponentsInChildren<ShipmentsAnswerCell>());
            }
            return answerCells;
        }

        private bool AnsweStateIsValid()
        {
            // Me fijo todas las respuestas
            int i = 0;
            for (; i < AnswerRowGameObjects.Length; i++)
            {
                GameObject answerRowGameObject = AnswerRowGameObjects[i];
                List<ShipmentsAnswerCell> answerCells = answerRowGameObject.GetComponentsInChildren<ShipmentsAnswerCell>().ToList();
                List<ShipmentsAnswerCell> shipmentsAnswerCells = answerCells.FindAll(e => e.Value == -1);
                // si hay entr 1 y 2 erroneas ya esta mal, xq esta incompleta
                if (shipmentsAnswerCells.Count > 0 && shipmentsAnswerCells.Count < answerCells.Count) return false;
                // si hay mas de 0 aca quiere decir que son todos erroneas, entonces si todas las que siguen
                // son erroneas puede ser una respuesta valida
                if (shipmentsAnswerCells.Count > 0) break;
            }

            // me fijo las siguientes, que en caso de que haya deberian estar vacias
            for (; i < AnswerRowGameObjects.Length; i++)
            {
                GameObject answerRowGameObject = AnswerRowGameObjects[i];
                List<ShipmentsAnswerCell> answerCells = answerRowGameObject.GetComponentsInChildren<ShipmentsAnswerCell>().ToList();
                List<ShipmentsAnswerCell> shipmentsAnswerCells = answerCells.FindAll(e => e.Value == -1);
                // como se que hay al menos una erronea, tienen que ser todas xq sino esta mal
                if (shipmentsAnswerCells.Count < answerCells.Count) return false;
            }

            return true;
        }

        public void OnClickMapPlace(int id, ShipmentNodeType type)
        {
            ShipmentsAnswerCell cell = GetCurrentAnswerCell();
            if (cell.Type == AnswerCellType.Numeric)
            {
                SoundController.GetController().PlayDropSound();
            }
            else
            {
                SoundController.GetController().PlayTypingSound();
                cell.Value = id;
                cell.GetComponent<Image>().sprite = AnswerCellSprites[id];
                List<ShipmentsAnswerCell> cells = GetAnswerCells();
                if (type == ShipmentNodeType.Other && _currentFocus + 2 < cells.Count && _currentFocus%3 == 1)
                {
                    ShipmentsAnswerCell cell2 = cells[_currentFocus + 2];
                    cell2.Value = id;
                    cell2.GetComponent<Image>().sprite = AnswerCellSprites[id];
                }
                else if (type == ShipmentNodeType.Other && _currentFocus -2 > 0 && _currentFocus % 3 == 0)
                {
                    ShipmentsAnswerCell cell2 = cells[_currentFocus - 2];
                    cell2.Value = id;
                    cell2.GetComponent<Image>().sprite = AnswerCellSprites[id];
                }
                FocusNextEmptyCell();

            }


            
        }

        private void FocusNextEmptyCell()
        {
            List<ShipmentsAnswerCell> cells = GetAnswerCells();
            for (int i = _currentFocus + 1; i < cells.Count; i++)
            {
                if (cells[i].Value != -1) continue;
                ChangeFocusCell(i);
                return;
            }
            for (int i = 1; i < _currentFocus; i++)
            {
                if (cells[i].Value != -1) continue;
                ChangeFocusCell(i);
                return;
            }
        }

        public void UpdateTryButton()
        {
            OkButton.interactable = AnsweStateIsValid();
        }

        private void AddCellLiseners()
        {
            List<ShipmentsAnswerCell> cells = GetAnswerCells();
            for (int i = 1; i < cells.Count; i++)
            {
                var i1 = i;
                cells[i].Value = -1;
                cells[i].GetComponent<Button>().onClick.AddListener(
                    () =>
                    {
                        MapPlace mapPlace = MapGenerator.Places.Find(e => e.Id == cells[i1].Value);
                        if(cells[i1].Value != 1 && mapPlace != null)
                        {
                            if (mapPlace.Type == ShipmentNodeType.Other && i1 + 2 < cells.Count && i1 % 3 == 1) cells[i1 + 2].Clear();
                            else if (mapPlace.Type == ShipmentNodeType.Other && i1 - 2 > 0 && i1 % 3 == 0) cells[i1 - 2].Clear();
                        }
                        
                        cells[i1].Clear();
                        ChangeFocusCell(i1);
                    }

                    );
            }
        }

        public void OnClickOk()
        {
            OkButton.enabled = false;
            PlaySoundClick();
            List<ShipmentEdge> edgeAnswers = new List<ShipmentEdge>();

            
            for (int i = 0; i < AnswerRowGameObjects.Length; i++)
            {
                GameObject answerRowGameObject = AnswerRowGameObjects[i];
                List<ShipmentsAnswerCell> answerCells = answerRowGameObject.GetComponentsInChildren<ShipmentsAnswerCell>().ToList();
                if (answerCells[0].Value == -1) break;
                ShipmentEdge shipmentEdge = new ShipmentEdge
                {
                    IdNodeA = answerCells[0].Value,
                    IdNodeB = answerCells[1].Value,
                    Length = answerCells[2].Value
                };
                edgeAnswers.Add(shipmentEdge);
            }

            for (int i = 0; i < 0; i++)
            {
                ShipmentEdge edge =
                    Model.Edges.Find(
                        e => (e.IdNodeA == edgeAnswers[i].IdNodeA && e.IdNodeB == edgeAnswers[i].IdNodeB) ||
                            (e.IdNodeB == edgeAnswers[i].IdNodeB && e.IdNodeA == edgeAnswers[i].IdNodeA));
                CheckEdgeAnswer(edge, edgeAnswers[i]);
                
                

            }

            if (Model.IsCorrectAnswer(edgeAnswers))
            {
                ShowRightAnswerAnimation();
            }
            else
            {
                ShowWrongAnswerAnimation();
            }
        }

        private void CheckEdgeAnswer(ShipmentEdge realEdge, ShipmentEdge answerEdge)
        {
            if (realEdge == null)
            {
                // no hay arista
                
            }
/*
            else

            {
               /* int answerValue = answerEdge.Length +  / Model.Scale;

                if(answerValue)
            }*/
        }

        public override void OnRightAnimationEnd()
        {
            OkButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(true);
        }

        public override void OnWrongAnimationEnd()
        {
            base.OnWrongAnimationEnd();
            OkButton.enabled = true;
        }
    }
}
