using UnityEngine;
using Assets.Scripts.Metrics;
using Assets.Scripts.Games;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.App
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private string[] name;
        [SerializeField]
        private string[] titles;
        [SerializeField]
        private string[] description;
        public string[] SpanishInstructionStrings;
        public string[] EnglishInstructionStrings;
        [SerializeField]
        private int levels;
        [SerializeField]
        private int area;
        [SerializeField] private string prefabName;
        [SerializeField] private List<int> _exercisesBySubLevel;

        public GameObject[] InstructionsSequence;


        private Game instance;


        public Sprite GetIcon()
        {
            return icon;
        }

        public string[] GetNames()
        {
            return name;
        }

        public string[] GetTitles()
        {
            return titles;
        }

        public string[] GetDescriptions()
        {
            return description;
        }

        public int GetLevels()
        {
            return levels;
        }

        public int GetArea()
        {
            return area;
        }    

        public int GetId()
        {
            return id;
        }

        public string GetPrefabName()
        {
            return prefabName;
        }

        public List<int> GetExercisesBySubLevel()
        {
            return _exercisesBySubLevel;
        }
    }
}
