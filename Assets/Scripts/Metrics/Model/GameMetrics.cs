using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Metrics
{

    [Serializable]
    public class GameMetrics{
        private const int VERSION = 1;
        private int lapsedSeconds, rightAnswers, wrongAnswers, stars, score;
        private int area;
        private int index;
        private int level;
        private string date;

        [NonSerialized]
        private int _realCorrectExercises;


        [OptionalField(VersionAdded = 2)]
        private int _currentSublevel;

        // round 0 is the first array, round 1 the second and so on.
        // if the array isn't initialized is beacuse the user don't reach 
        // this round
        [OptionalField(VersionAdded = 2)]
        private List<List<bool>> _answerBools;

        // the quantity of exercises by sublevel for reach the next sublevel
        [OptionalField(VersionAdded = 2)]
        private List<int> _exercisesBySublevel;

        [OptionalField(VersionAdded = 2)] private int _bonusTime;

        [OptionalField(VersionAdded = 2)] private Range _range; 

        public GameMetrics(int area, int index, int level, List<int> exercisesBySublevel = null)
        {
            this.index = index;
            this.area = area;
            this.level = level;
            stars = 0;
            lapsedSeconds = 0;
            rightAnswers = 0;
            wrongAnswers = 0;
            score = 0;
            date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            _exercisesBySublevel = exercisesBySublevel;
            _currentSublevel = 0;
            _answerBools = new List<List<bool>>(exercisesBySublevel != null ? exercisesBySublevel.Count : 10);
            _answerBools.Add(new List<bool>());
        }

        internal void Reset(){
            stars = 0;
            lapsedSeconds = 0;
            rightAnswers = 0;
            wrongAnswers = 0;
            score = 0;
        }    

        internal int GetStars(){
            return stars;
        }

        internal void AddWrongAnswer()
        {
            _answerBools[_currentSublevel].Add(false);
            this.wrongAnswers++;
        }

        internal void SetStars(int stars)
        {
            this.stars = stars;
        }

        internal void AddRightAnswer()
        {
            _answerBools[_currentSublevel].Add(true);
            this.rightAnswers++;
        }     

        internal int GetScore()
        {
            return score;
        }

        internal void SetLapsedSeconds(int lapsedSeconds)
        {
            this.lapsedSeconds = lapsedSeconds;
        }

        internal int GetWrongAnswers()
        {
            return wrongAnswers;
        }

        internal void SetScore(int score)
        {
            this.score = score;
        }

        internal int GetRightAnswers()
        {
            return rightAnswers;
        }

        internal string GetDate()
        {
            return date;
        }

        internal void SetDate(string date)
        {
            this.date = date;
        }

        internal int GetLapsedSeconds()
        {
            return lapsedSeconds;
        }

        internal void SetRightAnswers(int rightAnswers)
        {
            this.rightAnswers = rightAnswers;
        }

        internal void SetWrongAnswers(int wrongAnswers)
        {
            this.wrongAnswers = wrongAnswers;
        }

        internal int GetArea()
        {
            return area;
        }

        internal int GetIndex()
        {
            return index;
        }

        internal int GetLevel()
        {
            return level;
        }

        public bool CheckIfPassedToNextSubLevel(List<List<bool>> actualBuffer)
        {
            if (_exercisesBySublevel != null && _exercisesBySublevel.Count > 0)
            {
                /*   bool passedToNextLevel = false;
                   int correctExcercies = 0;
                   
                   for (int i = actualBuffer.Count - 1; i >= 0 && !passedToNextLevel; i--)
                   {
                       if (actualBuffer[i][0])
                       {
                           correctExcercies++;
                           passedToNextLevel = correctExcercies == _exercisesBySublevel[_currentSublevel];
                       }
                       else
                       {
                           correctExcercies--;
                       }
                   }*/
                _realCorrectExercises++;
                if (_realCorrectExercises == _exercisesBySublevel[_currentSublevel] && _currentSublevel < _exercisesBySublevel.Count - 1)
                {
                    _currentSublevel++;
                    _answerBools.Add(new List<bool>());
                    return true;
                }
            }
            return false;
        }

      

        public bool CheckIfDownSubLevel(List<List<bool>> actualBuffer)
        {
            if (_exercisesBySublevel != null && _exercisesBySublevel.Count > 0)
            {
                /*     bool downLevel = false;
                     int incorrectExcercies = 0;
                     _realCorrectExercises--;
                     for (int i = _answerBools[_currentSublevel].Count - 1; i >= 0 && !downLevel; i--)
                     {
                         if (!_answerBools[_currentSublevel][i])
                         {
                             incorrectExcercies++;
                             downLevel = incorrectExcercies == 2;
                         }
                         else
                         {
                             return false;
                         }
                     }*/
                _realCorrectExercises--;
                if (_currentSublevel != 0 && _realCorrectExercises < _exercisesBySublevel[_currentSublevel - 1])
                {
                    _currentSublevel--;
                    return true;
                }
               
            }
            return false;
        }

        public int GetCurrentSubLevel()
        {
            return _currentSublevel;
        }

        public void SetBonusTime(int bonusTime)
        {
            _bonusTime = bonusTime;
        }

        public void SetRange(Range range)
        {
            _range = range;
        }

        public Range GetRange()
        {
            return _range;
        }

        public int GetBonusTime()
        {
            return _bonusTime;
        }
    }
}